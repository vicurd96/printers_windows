﻿using Atom8.API.PrintSpool;
using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
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
        private TrabajoImpresionModel _jobSelected;
        public TrabajoImpresionModel JobSelected { get => _jobSelected; set { _jobSelected = value; RaisePropertyChanged("JobSelected"); } }

        //[DllImport("winspool.drv", EntryPoint = "SetJob")]
        //static extern int SetJob(IntPtr hPrinter, int JobId, int Level, ref byte pJob, int Command_Renamed);
        [DllImport("winspool.drv", EntryPoint = "GetJob", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetJob(IntPtr hPrinter, Int32 dwJobId, Int32 Level, IntPtr lpJob, Int32 cbBuf, ref Int32 lpbSizeNeeded);
        [DllImport("winspool.drv", EntryPoint = "GetJob", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetJob(Int32 hPrinter, Int32 dwJobId, Int32 Level, IntPtr lpJob, Int32 cbBuf, ref Int32 lpbSizeNeeded);
        [DllImport("winspool.drv", EntryPoint = "SetJob", SetLastError = true)]
        public static extern bool SetJob(IntPtr hPrinter, Int32 JobId, Int32 Level, IntPtr pJob, int Command_Renamed);
        [DllImport("winspool.drv",
          EntryPoint = "OpenPrinterA", SetLastError = true,
          CharSet = CharSet.Ansi, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(String pPrinterName,
        out IntPtr phPrinter,
        Int32 pDefault);
        [DllImport("winspool.drv",
            EntryPoint = "ClosePrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter
        (IntPtr hPrinter);

        public ColaImpresionViewModel(PrintServer servidor) : base(servidor)
        {
            ColaImpresionModel = new ObservableCollection<TrabajoImpresionModel>();
            Inicializar();
            Mediator.Subscribe(Metodo.ActualizarJobs, Actualizar);
            Mediator.Subscribe(Metodo.CancelarJob, Cancelar);
            Mediator.Subscribe(Metodo.CambiarPrioridadJob, CambiarPrioridad);
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

        private void Cancelar(object param = null)
        {
            if (JobSelected != null)
            {
                App.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    PrintSystemJobInfo infoJob = null;
                    PrintQueueCollection queues = servidor.GetPrintQueues();
                    foreach (PrintQueue queue in queues)
                    {
                        queue.Refresh();
                        try
                        {
                            infoJob = queue.GetJob(JobSelected.Id);
                            break;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    if(infoJob != null)
                        infoJob.Cancel();
                });
            }
        }

        private void CambiarPrioridad(object param = null)
        {
            if(JobSelected != null)
            {
                App.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    PrintSystemJobInfo infoJob = null;
                    PrintQueueCollection queues = servidor.GetPrintQueues();
                    PrintQueue printer = null;
                    IntPtr _printerHandle = IntPtr.Zero;
                    foreach (PrintQueue queue in queues)
                    {
                        queue.Refresh();
                        try
                        {
                            infoJob = queue.GetJob(JobSelected.Id);
                            printer = queue;
                            break;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    if (infoJob != null)
                    {
                        Accion accion = (Accion)param;
                        if ((infoJob.Priority < PrintJobPriority.Maximum || accion != Accion.SubirPrioridad)
                            && (infoJob.Priority > PrintJobPriority.Minimum || accion != Accion.BajarPrioridad))
                        {
                            OpenPrinter(printer.Name, out _printerHandle, 0);
                            if (_printerHandle == IntPtr.Zero)
                            {
                                throw new Exception("OpenPrinter() Failed with error code " + Marshal.GetLastWin32Error());
                            }
                            JOB_INFO_2 workJob = GetJobInfo(_printerHandle, infoJob.JobIdentifier);
                            switch (accion)
                            {
                                case Accion.SubirPrioridad:
                                    workJob.Priority++;
                                    break;
                                case Accion.BajarPrioridad:
                                    workJob.Priority--;
                                    break;
                            }
                            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(JOB_INFO_2)));
                            Marshal.StructureToPtr(workJob, ptr, false);
                            if(!SetJob(_printerHandle, infoJob.JobIdentifier, 2, ptr, 0))
                            {
                                switch (Marshal.GetLastWin32Error())
                                {
                                    case (int)Win32Error.ERROR_UNKNOWN_PRINTPROCESSOR:
                                        break;
                                }
                                throw new Exception("SetJob() Failed with error code " + Marshal.GetLastWin32Error());
                            }
                            //if (!SetJob(_printerHandle, infoJob.JobIdentifier, 1, ptr, 0))
                            //{
                            //    throw new Exception("SetJob() Failed with error code " + Marshal.GetLastWin32Error());
                            //}
                            ClosePrinter(_printerHandle);
                        }
                    }
                });
            }
        }

        private JOB_INFO_2 GetJobInfo(IntPtr _printerHandle, int jobId)
        {
            JOB_INFO_2 info;
            Int32 BytesWritten = default(Int32);
            IntPtr ptBuf = default(IntPtr);

            if (!GetJob(_printerHandle, jobId, 2, ptBuf, 0, ref BytesWritten))
            {
                if (BytesWritten == 0)
                {
                    throw new Exception("GetJob for JOB_INFO_2 failed on handle: " + _printerHandle.ToString() + " for job: " + jobId);
                }
            }

            //Allocate a buffer the right size
            if (BytesWritten > 0)
            {
                ptBuf = Marshal.AllocHGlobal(BytesWritten * 2);
            }

            if (!GetJob(_printerHandle, jobId, 2, ptBuf, BytesWritten, ref BytesWritten))
            {
                throw new Exception("GetJob for JOB_INFO_2 failed on handle: " + _printerHandle.ToString() + " for job: " + jobId);
            }
            else
            {
                info = (JOB_INFO_2)Marshal.PtrToStructure(ptBuf, typeof(JOB_INFO_2));
            }

            //\\ Free the allocated memory
            Marshal.FreeHGlobal(ptBuf);
            return info;
        }
    }
}
