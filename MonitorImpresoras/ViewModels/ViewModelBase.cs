using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected PrintServer servidor;
        public ViewModelBase(PrintServer servidor) 
        {
            this.servidor = servidor;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
