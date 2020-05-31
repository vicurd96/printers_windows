using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace MonitorImpresoras.Models
{
    public class LayoutBaseModel : INotifyPropertyChanged
    {
        private UserControl _impresorasView, _procesosView, _colaView;

        private List<UserControl> _pageViewModels;
        public List<UserControl> PageViewModels {
            get {
                if (_pageViewModels == null)
                    _pageViewModels = new List<UserControl>();

                return _pageViewModels;
            }

        }
        public UserControl ImpresorasView {
            get {
                return _impresorasView;
            }
            set {
                _impresorasView = value;
                RaisePropertyChanged("ImpresorasView");
            }
        }

        public UserControl ColaView {
            get {
                return _colaView;
            }
            set {
                _colaView = value;
                RaisePropertyChanged("ColaView");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
