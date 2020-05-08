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

        public MainViewModel(LocalPrintServer local, PrintServer network) : base(local, network)
        {
            MainModel = new MainModel();
            LayoutBaseModel.PageViewModels.Add(new ImpresorasView(local, network));
            LayoutBaseModel.PageViewModels.Add(new ColaImpresionView(local, network));
            LayoutBaseModel.PageViewModels.Add(new ProcesosImpresionView(local, network));
            ChangeViewModel(LayoutBaseModel.PageViewModels[0]);
        }
    }
}
