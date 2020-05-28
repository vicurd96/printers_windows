using Atom8.API.PrintSpool;
using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MonitorImpresoras.ViewModels
{
    public class ColaImpresionViewModel : ViewModelBase
    {
        public ObservableCollection<TrabajoImpresionModel> ColaImpresionModel { get; set; }
        public CollectionView CollectionView { get => (CollectionView)CollectionViewSource.GetDefaultView(ColaImpresionModel); }
        private static object _syncLock = new object();

        public ColaImpresionViewModel(PrintServer servidor) : base(servidor)
        {
            ColaImpresionModel = new ObservableCollection<TrabajoImpresionModel>();
            Inicializar();
            //BindingOperations.EnableCollectionSynchronization(ColaImpresionModel, _syncLock);
            Mediator.Subscribe("ActualizarColaImpresion", Actualizar);
        }

        private void Inicializar()
        {
            PrintQueueCollection queues = servidor.GetPrintQueues();
            foreach(PrintQueue queue in queues)
            {
                queue.Refresh();
                PrintJobInfoCollection jobs = queue.GetPrintJobInfoCollection();
                foreach(PrintSystemJobInfo job in jobs)
                {
                    ColaImpresionModel.Add(new TrabajoImpresionModel
                    {
                        Id = job.JobIdentifier,
                        Name = job.Name,
                        JobStatus = (JOBSTATUS)job.JobStatus,
                        NumPages = job.NumberOfPages,
                        Owner = job.Submitter,
                        Priority = ((int)job.Priority).ToString()
                    });
                }
            }

        }

        private void Actualizar(object job = null)
        {
            if (job != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    TrabajoImpresionModel PrintJob = (TrabajoImpresionModel)job;
                    PrintQueueCollection queues = new PrintServer().GetPrintQueues();
                    PrintSystemJobInfo infoJob = null;
                    foreach (PrintQueue queue in queues)
                    {
                        queue.Refresh();
                        try
                        {
                            infoJob = queue.GetJob(PrintJob.Id);
                            break;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    TrabajoImpresionModel model = ColaImpresionModel.FirstOrDefault(c => c.Id == PrintJob.Id);
                    if (infoJob != null)
                    {
                        if (model != null)
                        {
                            model.JobStatus = (JOBSTATUS)infoJob.JobStatus;
                            model.Estado = ((int)infoJob.JobStatus).ToString();
                        }
                        else
                        {
                            ColaImpresionModel.Add(new TrabajoImpresionModel
                            {
                                Id = infoJob.JobIdentifier,
                                Estado = ((int)infoJob.JobStatus).ToString(),
                                JobStatus = (JOBSTATUS)infoJob.JobStatus,
                                Name = infoJob.Name,
                                NumPages = infoJob.NumberOfPages,
                                Owner = infoJob.Submitter,
                                Priority = ((int)infoJob.Priority).ToString()
                            });
                        }
                    }
                    else
                    {
                        if (model != null)
                        {
                            ColaImpresionModel.Remove(model);
                        }
                    }
                    CollectionView.Refresh();
                    GC.Collect();
                });
            }
        }
    }
}
