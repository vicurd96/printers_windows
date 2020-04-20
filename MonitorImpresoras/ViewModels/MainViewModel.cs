using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MonitorImpresoras.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainModel MainModel { get; }

        private ICommand _buttonCommand;

        public ICommand ButtonCommand {
            get {
                if(_buttonCommand == null)
                {
                    _buttonCommand = new RelayCommand((param) =>
                    {
                        MainModel.Texto = "Cambio de texto";
                    });
                }
                return _buttonCommand;
            }
        }

        public MainViewModel()
        {
            MainModel = new MainModel();
            MainModel.Texto = "Esto es una prueba";
        }
    }
}
