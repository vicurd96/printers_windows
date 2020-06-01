using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MonitorImpresoras.ViewModels
{
    public class ImpresorasViewModel : ViewModelBase
    {
        public ObservableCollection<ImpresorasModel> ImpresorasModel { get; }
        private ImpresorasModel _printerSelected;
        public ImpresorasModel PrinterSelected { get => _printerSelected; set { _printerSelected = value; RaisePropertyChanged("PrinterSelected"); } }
        private List<PrintQueueMonitor> listMonitor = new List<PrintQueueMonitor>();
        public CollectionView CollectionView { get => (CollectionView)CollectionViewSource.GetDefaultView(ImpresorasModel); }

        public ImpresorasViewModel(PrintServer servidor) : base(servidor)
        {
            ImpresorasModel = new ObservableCollection<ImpresorasModel>();
            Inicializar();
            Mediator.Subscribe(Metodo.CambiarStatusPrinter, CambiarStatus);
        }

        public void Inicializar()
        {
            PrintQueueCollection queues = servidor.GetPrintQueues();

            foreach (PrintQueue queue in queues)
            {
                ImpresorasModel.Add(new ImpresorasModel { 
                    Nombre = queue.Name, 
                    isCompartida = queue.IsShared, 
                    Status = (int)queue.QueueStatus, 
                    Puerto = queue.QueuePort.Name,
                    Prioridad = queue.Priority
                });
                PrintQueueMonitor pqm = new PrintQueueMonitor(queue.Name, servidor);
                listMonitor.Add(pqm);
                pqm.OnJobStatusChange += new PrintJobStatusChanged(pqm_OnJobStatusChange);
                pqm.OnPrinterStatusChange += new PrinterStatusChanged(pqm_OnPrinterStatusChange);
            }
            CollectionView.Refresh();
        }

        private void pqm_OnJobStatusChange(object Sender, PrintJobChangeEventArgs e)
        {
            Actualizar();
            if (!string.IsNullOrEmpty(e.JobName))
            {
                Mediator.Notify(Metodo.ActualizarJobs, new TrabajoImpresionModel
                {
                    Id = e.JobID,
                    Name = e.JobName,
                    JobStatus = e.JobStatus,
                    NumPages = e.JobNumPages,
                    Owner = e.JobOwner,
                    Estado = ((int)e.JobStatus).ToString(),
                    Priority = e.JobPriority.ToString(),
                    PrinterName = e.JobPrinter
                });
            }
        }

        private void pqm_OnPrinterStatusChange(object Sender, PrinterChangeEventArgs e)
        {
            Actualizar();
        }

        private void Actualizar()
        {
            App.Current.Dispatcher.BeginInvoke(delegate
            {
                PrintQueueCollection queues = servidor.GetPrintQueues();
                ImpresorasModel.Clear();
                foreach (PrintQueue queue in queues)
                {
                    ImpresorasModel.Add(new ImpresorasModel
                    {
                        Nombre = queue.Name,
                        isCompartida = queue.IsShared,
                        Status = (int)queue.QueueStatus,
                        Puerto = queue.QueuePort.Name,
                        Prioridad = queue.Priority
                    });
                }
                CollectionView.Refresh();
            });
        }

        private void CambiarStatus(object param = null)
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                if (PrinterSelected != null && param != null) {
                    using(PrintQueue printQueue = new PrintQueue(servidor, PrinterSelected.Nombre, PrintSystemDesiredAccess.AdministratePrinter)){
                        if(printQueue != null)
                        {
                            Accion accion = (Accion)param;
                            try
                            {
                                switch (accion)
                                {
                                    case Accion.Pausar:
                                        if (printQueue.QueueStatus != PrintQueueStatus.Paused)
                                            printQueue.Pause();
                                        break;
                                    case Accion.Reanudar:
                                        if (printQueue.QueueStatus == PrintQueueStatus.Paused)
                                            printQueue.Resume();
                                        break;
                                    case Accion.SubirPrioridad:
                                        if (printQueue.Priority < (int)PrintJobPriority.Maximum)
                                        {
                                            printQueue.Priority++;
                                            printQueue.Commit();
                                            Actualizar();
                                        }
                                        break;
                                    case Accion.BajarPrioridad:
                                        if (printQueue.Priority > (int)PrintJobPriority.Minimum)
                                        {
                                            printQueue.Priority--;
                                            printQueue.Commit();
                                            Actualizar();
                                        }
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                string messageError = "Ocurrió un error inesperado al cambiar alguna propiedad de la impresora";
                                string captionError = "Cambiar impresora";
                                MessageBox.Show(messageError, captionError, MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            });
        }
    }
}
