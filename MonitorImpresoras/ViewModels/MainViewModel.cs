using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainModel MainModel { get; }

        public MainViewModel()
        {
            MainModel = new MainModel();
            MainModel.Texto = "Esto es una prueba";
        }
    }
}
