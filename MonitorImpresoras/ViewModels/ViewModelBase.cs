using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
