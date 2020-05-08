using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected PrintServer network;
        protected LocalPrintServer local;
        public ViewModelBase(LocalPrintServer local, PrintServer network) 
        {
            this.network = network;
            this.local = local;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
