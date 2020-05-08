using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    public class ColaImpresionViewModel : ViewModelBase
    {
        public ColaImpresionModel ColaImpresionModel { get; }

        public ColaImpresionViewModel(LocalPrintServer local, PrintServer network) : base(local, network)
        {

        }
    }
}
