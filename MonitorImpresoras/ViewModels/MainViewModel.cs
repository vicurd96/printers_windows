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
