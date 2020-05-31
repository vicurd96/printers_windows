using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using MonitorImpresoras.Views;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MonitorImpresoras.ViewModels
{
    public class MainViewModel : LayoutBaseViewModel
    {
        public MainModel MainModel { get; }
        #region COMANDOS
        private ICommand _salirCommand;
        private ICommand _pausarPrinterCommand, _reanudarPrinterCommand;
        private ICommand _subirPrioridadPrinterCommand, _bajarPrioridadPrinterCommand;
        private ICommand _pausarJobCommand, _reanudarJobCommand, _reiniciarJobCommand;
        private ICommand _cancelarJobCommand, _subirPrioridadJobCommand, _bajarPrioridadJobCommand;
        public ICommand SalirCommand {
            get {
                if(_salirCommand == null)
                {
                    _salirCommand = new RelayCommand((_) =>
                    {
                        Application.Current.Shutdown();
                    });
                }
                return _salirCommand;
            }
        }
        public ICommand PausarPrinterCommand {
            get {
                if(_pausarPrinterCommand == null)
                {
                    _pausarPrinterCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusPrinter, Accion.Pausar);
                    });
                }
                return _pausarPrinterCommand;
            }
        }
        public ICommand ReanudarPrinterCommand {
            get {
                if (_reanudarPrinterCommand == null)
                {
                    _reanudarPrinterCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusPrinter, Accion.Reanudar);
                    });
                }
                return _reanudarPrinterCommand;
            }
        }
        public ICommand SubirPrioridadPrinterCommand {
            get {
                if (_subirPrioridadPrinterCommand == null)
                {
                    _subirPrioridadPrinterCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusPrinter, Accion.SubirPrioridad);
                    });
                }
                return _subirPrioridadPrinterCommand;
            }
        }
        public ICommand BajarPrioridadPrinterCommand {
            get {
                if (_bajarPrioridadPrinterCommand == null)
                {
                    _bajarPrioridadPrinterCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusPrinter, Accion.BajarPrioridad);
                    });
                }
                return _bajarPrioridadPrinterCommand;
            }
        }
        public ICommand PausarJobCommand {
            get {
                if (_pausarJobCommand == null)
                {
                    _pausarJobCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusJob, Accion.Pausar);
                    });
                }
                return _pausarJobCommand;
            }
        }
        public ICommand ReanudarJobCommand {
            get {
                if (_reanudarJobCommand == null)
                {
                    _reanudarJobCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusJob, Accion.Reanudar);
                    });
                }
                return _reanudarJobCommand;
            }
        }
        public ICommand ReiniciarJobCommand {
            get {
                if (_reiniciarJobCommand == null)
                {
                    _reiniciarJobCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarStatusJob, Accion.Reiniciar);
                    });
                }
                return _reiniciarJobCommand;
            }
        }
        public ICommand CancelarJobCommand {
            get {
                if(_cancelarJobCommand == null)
                {
                    _cancelarJobCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CancelarJob);
                    });
                }
                return _cancelarJobCommand;
            }
        }
        public ICommand SubirPrioridadJobCommand {
            get {
                if (_subirPrioridadJobCommand == null)
                {
                    _subirPrioridadJobCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarPrioridadJob, Accion.SubirPrioridad);
                    });
                }
                return _subirPrioridadJobCommand;
            }
        }
        public ICommand BajarPrioridadJobCommand {
            get {
                if (_bajarPrioridadJobCommand == null)
                {
                    _bajarPrioridadJobCommand = new RelayCommand((_) =>
                    {
                        Mediator.Notify(Metodo.CambiarPrioridadJob, Accion.BajarPrioridad);
                    });
                }
                return _bajarPrioridadJobCommand;
            }
        }
        #endregion

        public MainViewModel(PrintServer servidor) : base(servidor)
        {
            MainModel = new MainModel();
            LayoutBaseModel.ImpresorasView = new ImpresorasView(servidor);
            LayoutBaseModel.ColaView = new ColaImpresionView(servidor);
        }
    }
}
