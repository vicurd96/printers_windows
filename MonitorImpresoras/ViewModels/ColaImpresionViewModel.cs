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
        public List<ColaImpresionModel> ColaImpresionModel { get; } = new List<ColaImpresionModel>();
        public CollectionView CollectionView { get => (CollectionView)CollectionViewSource.GetDefaultView(ColaImpresionModel); }

        public ColaImpresionViewModel(PrintServer servidor) : base(servidor)
        {
            Inicializar();
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
                    ColaImpresionModel.Add(new ColaImpresionModel { 
                        Id = job.JobIdentifier,
                        Name = job.Name,
                        Status = (JOBSTATUS)job.JobStatus
                    });
                }
            }

        }

        private void Actualizar(object job = null)
        {
            if (job != null)
            {
                ColaImpresionModel PrintJob = (ColaImpresionModel)job;
                if (PrintJob != null)
                {
                    ColaImpresionModel model = ColaImpresionModel.FirstOrDefault(c => c.Id == PrintJob.Id);
                    if (model != null)
                        model.Status = PrintJob.Status;
                    else
                    {
                        Dispatcher.CurrentDispatcher.BeginInvoke(delegate
                        {
                            ColaImpresionModel.Add(PrintJob);
                        });
                            
                    }
                    CollectionView.Refresh();
                }
            }
        }
    }
}
