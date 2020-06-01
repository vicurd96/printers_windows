using Atom8.API.PrintSpool;
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
        [DllImport("winspool.drv", EntryPoint = "GetJob", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetJob(IntPtr hPrinter, Int32 dwJobId, Int32 Level, IntPtr lpJob, Int32 cbBuf, ref Int32 lpbSizeNeeded);
        [DllImport("winspool.drv", EntryPoint = "GetJob", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetJob(Int32 hPrinter, Int32 dwJobId, Int32 Level, IntPtr lpJob, Int32 cbBuf, ref Int32 lpbSizeNeeded);
        [DllImport("winspool.drv", EntryPoint = "GetJob", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetJobW(IntPtr hPrinter, Int32 dwJobId, Int32 Level, IntPtr lpJob, Int32 cbBuf, ref Int32 lpbSizeNeeded);
        [DllImport("winspool.drv", EntryPoint = "SetJobA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetJob(IntPtr hPrinter, Int32 JobId, Int32 Level, IntPtr pJob, int Command_Renamed);
        [DllImport("winspool.drv",
          EntryPoint = "OpenPrinterA", SetLastError = true,
          CharSet = CharSet.Ansi, ExactSpelling = true,
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(String pPrinterName,
        out IntPtr phPrinter,
        ref PRINTER_DEFAULTS pDefault);
        [DllImport("winspool.drv",
            EntryPoint = "ClosePrinter",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter
        (IntPtr hPrinter);
        [DllImport("Winspool.drv", SetLastError = true, EntryPoint = "EnumJobs", CharSet = CharSet.Unicode)]
        public static extern bool EnumJobs(
           IntPtr hPrinter,
           UInt32 FirstJob,
           UInt32 NoJobs,
           UInt32 Level,
           IntPtr pJob,
           UInt32 cbBuf,
           out UInt32 pcbNeeded,
           out UInt32 pcReturned
        );

        public ColaImpresionViewModel(PrintServer servidor) : base(servidor)
        {
            ColaImpresionModel = new ObservableCollection<TrabajoImpresionModel>();
            Inicializar();
            Mediator.Subscribe(Metodo.ActualizarJobs, Actualizar);
            Mediator.Subscribe(Metodo.CancelarJob, Cancelar);
            Mediator.Subscribe(Metodo.CambiarPrioridadJob, CambiarPrioridad);
            Mediator.Subscribe(Metodo.CambiarStatusJob, CambiarStatus);
        }

        private void Inicializar()
        {
            uint firstJob = 0u, noJobs = 99u, level = 1u;
            PRINTER_DEFAULTS pDefaults = new PRINTER_DEFAULTS
            {
                DesiredAccess = (uint)PrintSystemDesiredAccess.UsePrinter,
                pDatatype = IntPtr.Zero,
                pDevMode = IntPtr.Zero
            };
            PrintQueueCollection queues = servidor.GetPrintQueues();
            foreach (PrintQueue queue in queues)
            {
                IntPtr _printerHandle = IntPtr.Zero;
                uint needed, returned;
                OpenPrinter(queue.Name, out _printerHandle, ref pDefaults);
                if (_printerHandle == IntPtr.Zero)
                    continue;
                bool eJobs = EnumJobs(_printerHandle, firstJob, noJobs, level, IntPtr.Zero, 0, out needed, out returned);
                if(!eJobs)
                {
                    int error = Marshal.GetLastWin32Error();
                    if (error != 122) continue;
                }
                IntPtr pJob = Marshal.AllocHGlobal((int)needed);
                uint bytesCopied, structsCopied;
                bool eJobs2 = EnumJobs(
                    _printerHandle, firstJob, noJobs, level, pJob, needed, out bytesCopied, out structsCopied);
                if (eJobs2)
                {
                    int sizeOf = Marshal.SizeOf(typeof(JOB_INFO_1));
                    IntPtr pStruct = pJob;
                    for (int i = 0; i < structsCopied; i++)
                    {
                        JOB_INFO_1 infoJob = (JOB_INFO_1)Marshal.PtrToStructure(pStruct, typeof(JOB_INFO_1));
                        ColaImpresionModel.Add(new TrabajoImpresionModel
                        {
                            Id = (int)infoJob.JobId,
                            Estado = ((int)infoJob.Status).ToString(),
                            JobStatus = (JOBSTATUS)infoJob.Status,
                            Name = infoJob.pDocument,
                            NumPages = (int)infoJob.TotalPages,
                            Owner = infoJob.pUserName,
                            Priority = ((int)infoJob.Priority).ToString(),
                            PrinterName = infoJob.pPrinterName
                        });
                        pStruct += sizeOf;
                    }
                    Marshal.FreeHGlobal(pJob);
                }
                //queue.Refresh();
                //PrintJobInfoCollection jobs = queue.GetPrintJobInfoCollection();
                //foreach (PrintSystemJobInfo job in jobs)
                //{
                //    ColaImpresionModel.Add(new TrabajoImpresionModel
                //    {
                //        Id = job.JobIdentifier,
                //        Name = job.Name,
                //        JobStatus = (JOBSTATUS)job.JobStatus,
                //        NumPages = job.NumberOfPages,
                //        Owner = job.Submitter,
                //        Priority = ((int)job.Priority).ToString()
                //    });
                //}
            }
            CollectionView.Refresh();

        }

        private void Actualizar(object job = null)
        {
            if (job != null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    TrabajoImpresionModel PrintJob = (TrabajoImpresionModel)job;
                    PrintQueueCollection queues = servidor.GetPrintQueues();
                    IntPtr _printerHandle = IntPtr.Zero;
                    PRINTER_DEFAULTS pDefaults = new PRINTER_DEFAULTS
                    {
                        DesiredAccess = (uint)PrintSystemDesiredAccess.UsePrinter,
                        pDatatype = IntPtr.Zero,
                        pDevMode = IntPtr.Zero
                    };
                    TrabajoImpresionModel model = ColaImpresionModel.FirstOrDefault(c => c.Id == PrintJob.Id);
                    OpenPrinter(PrintJob.PrinterName, out _printerHandle, ref pDefaults);
                    if (_printerHandle == IntPtr.Zero)
                    {
                        if (model != null)
                        {
                            ColaImpresionModel.Remove(model);
                        }
                        return;
                    }
                    JOB_INFO_1 infoJob = new JOB_INFO_1();
                    try
                    {
                        infoJob = GetJobInfoUnicode(_printerHandle, PrintJob.Id);
                    }
                    catch (Exception)
                    {
                        if (model != null)
                        {
                            ColaImpresionModel.Remove(model);
                        }
                        return;
                    }
                    if (model != null)
                    {
                        if ((JOBSTATUS)infoJob.Status == JOBSTATUS.JOB_STATUS_COMPLETE ||
                            ((JOBSTATUS)infoJob.Status == JOBSTATUS.JOB_STATUS_PRINTED) ||
                            (JOBSTATUS)infoJob.Status == JOBSTATUS.JOB_STATUS_DELETED)
                            ColaImpresionModel.Remove(model);
                        else
                        {
                            model.JobStatus = (JOBSTATUS)infoJob.Status;
                            model.Estado = ((int)infoJob.Status).ToString();
                            model.Priority = ((int)infoJob.Priority).ToString();
                        }
                    }
                    else
                    {
                        ColaImpresionModel.Add(new TrabajoImpresionModel
                        {
                            Id = (int)infoJob.JobId,
                            Estado = ((int)infoJob.Status).ToString(),
                            JobStatus = (JOBSTATUS)infoJob.Status,
                            Name = infoJob.pDocument,
                            NumPages = (int)infoJob.TotalPages,
                            Owner = infoJob.pUserName,
                            Priority = ((int)infoJob.Priority).ToString(),
                            PrinterName = infoJob.pPrinterName
                        });
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

        private void CambiarStatus(object param = null)
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
                    if (infoJob != null && param != null) {
                        Accion accion = (Accion)param;
                        switch (accion)
                        {
                            case Accion.Pausar:
                                if (infoJob.JobStatus != PrintJobStatus.Paused)
                                    infoJob.Pause();
                                break;
                            case Accion.Reanudar:
                                if (infoJob.JobStatus == PrintJobStatus.Paused)
                                    infoJob.Resume();
                                break;
                            case Accion.Reiniciar:
                                infoJob.Restart();
                                break;
                        }
                    }
                });
            }
        }

        private void CambiarPrioridad(object param = null)
        {
            if(JobSelected != null)
            {
                App.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    PrintQueueCollection queues = servidor.GetPrintQueues();
                    IntPtr _printerHandle = IntPtr.Zero;
                    PRINTER_DEFAULTS pDefaults = new PRINTER_DEFAULTS
                    {
                        DesiredAccess = (uint)PrintSystemDesiredAccess.AdministratePrinter,
                        pDatatype = IntPtr.Zero,
                        pDevMode = IntPtr.Zero
                    };
                    OpenPrinter(JobSelected.PrinterName, out _printerHandle, ref pDefaults);
                    if (_printerHandle == IntPtr.Zero)
                    {
                        string messageError = "Ocurrió un error inesperado al abrir la impresora";
                        string captionError = "Cambiar prioridad";
                        MessageBox.Show(messageError, captionError, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    JOB_INFO_1 infoJob = new JOB_INFO_1();
                    try
                    {
                        infoJob = GetJobInfo(_printerHandle, JobSelected.Id);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    Accion accion = (Accion)param;
                    if ((infoJob.Priority < (uint)PrintJobPriority.Maximum || accion != Accion.SubirPrioridad)
                        && (infoJob.Priority > (uint)PrintJobPriority.Minimum || accion != Accion.BajarPrioridad))
                    {
                        switch (accion)
                        {
                            case Accion.SubirPrioridad:
                                infoJob.Priority += 1;
                                break;
                            case Accion.BajarPrioridad:
                                infoJob.Priority -= 1;
                                break;
                        }
                        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(JOB_INFO_1)));
                        Marshal.StructureToPtr(infoJob, ptr, false);
                        if(!SetJob(_printerHandle, (int)infoJob.JobId, 1, ptr, 0))
                        {
                            string messageError = "Ocurrió un error inesperado al modificar el trabajo de impresión";
                            string captionError = "Cambiar prioridad";
                            MessageBox.Show(messageError, captionError, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        ClosePrinter(_printerHandle);
                    }
                });
            }
        }

        private JOB_INFO_1 GetJobInfo(IntPtr _printerHandle, int jobId)
        {
            JOB_INFO_1 info = new JOB_INFO_1();
            Int32 BytesWritten = default(Int32);
            IntPtr ptBuf = default(IntPtr);

            if (!GetJob(_printerHandle, jobId, 1, ptBuf, 0, ref BytesWritten))
            {
                if (BytesWritten == 0)
                {
                    string messageError = "Ocurrió un error inesperado al acceder a la información del trabajo de impresión";
                    string captionError = "Cambiar prioridad";
                    MessageBox.Show(messageError, captionError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            //Allocate a buffer the right size
            if (BytesWritten > 0)
            {
                ptBuf = Marshal.AllocHGlobal(BytesWritten);
            }

            if (!GetJob(_printerHandle, jobId, 1, ptBuf, BytesWritten, ref BytesWritten))
            {
                string messageError = "Ocurrió un error inesperado al acceder a la información del trabajo de impresión";
                string captionError = "Cambiar prioridad";
                MessageBox.Show(messageError, captionError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                info = (JOB_INFO_1)Marshal.PtrToStructure(ptBuf, typeof(JOB_INFO_1));
            }

            //\\ Free the allocated memory
            Marshal.FreeHGlobal(ptBuf);
            return info;
        }

        private JOB_INFO_1 GetJobInfoUnicode(IntPtr _printerHandle, int jobId)
        {
            JOB_INFO_1 info = new JOB_INFO_1();
            Int32 BytesWritten = default(Int32);
            IntPtr ptBuf = default(IntPtr);

            if (!GetJobW(_printerHandle, jobId, 1, ptBuf, 0, ref BytesWritten))
            {
                if (BytesWritten == 0)
                {
                    throw new Exception();
                }
            }

            //Allocate a buffer the right size
            if (BytesWritten > 0)
            {
                ptBuf = Marshal.AllocHGlobal(BytesWritten);
            }

            if (!GetJobW(_printerHandle, jobId, 1, ptBuf, BytesWritten, ref BytesWritten))
            {
                throw new Exception();
            }
            else
            {
                info = (JOB_INFO_1)Marshal.PtrToStructure(ptBuf, typeof(JOB_INFO_1));
            }

            //\\ Free the allocated memory
            Marshal.FreeHGlobal(ptBuf);
            return info;
        }
    }
}
