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
        public List<TrabajoImpresionModel> ColaImpresionModel { get; } = new List<TrabajoImpresionModel>();
        public CollectionView CollectionView { get => (CollectionView)CollectionViewSource.GetDefaultView(ColaImpresionModel); }
        private static object _syncLock = new object();

        public ColaImpresionViewModel(PrintServer servidor) : base(servidor)
        {
            Inicializar();
            BindingOperations.EnableCollectionSynchronization(ColaImpresionModel, _syncLock);
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
                    lock (_syncLock)
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

        }

        private void Actualizar(object job = null)
        {
            if (job != null)
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
                if(infoJob != null)
                {
                    TrabajoImpresionModel model = ColaImpresionModel.FirstOrDefault(c => c.Id == PrintJob.Id);
                    if (model != null)
                        model.JobStatus = PrintJob.JobStatus;
                    else
                    {
                        lock (_syncLock)
                        {
                            ColaImpresionModel.Add(new TrabajoImpresionModel { 
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
                    CollectionView.Refresh();
                }
                GC.Collect();
            }
        }
    }
}
