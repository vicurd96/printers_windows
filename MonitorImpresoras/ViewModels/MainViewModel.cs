using MonitorImpresoras.Helpers;
using MonitorImpresoras.Models;
using MonitorImpresoras.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace MonitorImpresoras.ViewModels
{
    public class MainViewModel : LayoutBaseViewModel
    {
        public MainModel MainModel { get; }
        #region COMANDOS
        private ICommand _onLoad;

        public ICommand OnLoad {
            get {
                if(_onLoad == null)
                {
                    _onLoad = new RelayCommand((param) =>
                    {
                        List<UserControl> ListViews = new List<UserControl>();
                        ListViews.Add(new ColaImpresionView());
                        ListViews.Add(new ProcesosImpresionView());
                        LayoutBaseModel.PageViewModels.AddRange(ListViews);
                    });
                }
                return _onLoad;
            }
        }
        #endregion

        public MainViewModel() : base()
        {
            MainModel = new MainModel();
        }
    }
}
