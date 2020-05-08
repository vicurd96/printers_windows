using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    public class ProcesosImpresionViewModel : ViewModelBase
    {
        public ProcesosImpresionModel ProcesosImpresionModel { get; }

        public ProcesosImpresionViewModel(LocalPrintServer local, PrintServer network) : base(local, network)
        {

        }
    }
}
