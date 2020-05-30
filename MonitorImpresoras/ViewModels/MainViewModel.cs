using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using MonitorImpresoras.Views;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace MonitorImpresoras.ViewModels
{
    public class MainViewModel : LayoutBaseViewModel
    {
        public MainModel MainModel { get; }
        #region COMANDOS
        private ICommand _cancelarJobCommand, _subirPrioridadCommand, _bajarPrioridadCOmmand;
        public ICommand CancelarJobCommand {
            get {
                if(_cancelarJobCommand == null)
                {
                    _cancelarJobCommand = new RelayCommand((param) =>
                    {
                        Mediator.Notify(Metodo.CancelarJob);
                    });
                }
                return _cancelarJobCommand;
            }
        }
        public ICommand SubirPrioridadCommand {
            get {
                if (_subirPrioridadCommand == null)
                {
                    _subirPrioridadCommand = new RelayCommand((param) =>
                    {
                        Mediator.Notify(Metodo.CambiarPrioridadJob, Accion.SubirPrioridad);
                    });
                }
                return _subirPrioridadCommand;
            }
        }
        public ICommand BajarPrioridadCommand {
            get {
                if (_bajarPrioridadCOmmand == null)
                {
                    _bajarPrioridadCOmmand = new RelayCommand((param) =>
                    {
                        Mediator.Notify(Metodo.CambiarPrioridadJob, Accion.SubirPrioridad);
                    });
                }
                return _bajarPrioridadCOmmand;
            }
        }
        #endregion

        public MainViewModel(PrintServer servidor) : base(servidor)
        {
            MainModel = new MainModel();
            LayoutBaseModel.ImpresorasView = new ImpresorasView(servidor);
            LayoutBaseModel.ProcesosView = new ProcesosImpresionView(servidor);
            LayoutBaseModel.ColaView = new ColaImpresionView(servidor);
        }
    }
}
