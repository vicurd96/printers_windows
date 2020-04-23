using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace MonitorImpresoras.Models
{
    public class LayoutBaseModel : INotifyPropertyChanged
    {
        private UserControl _previousPageViewModel, _currentPageViewModel;

        private List<UserControl> _pageViewModels;
        public List<UserControl> PageViewModels {
            get {
                if (_pageViewModels == null)
                    _pageViewModels = new List<UserControl>();

                return _pageViewModels;
            }

        }
        public UserControl CurrentPageViewModel {
            get {
                return _currentPageViewModel;
            }
            set {
                PreviousPageViewModel = _currentPageViewModel;
                _currentPageViewModel = value;
                RaisePropertyChanged("CurrentPageViewModel");
            }
        }

        public UserControl PreviousPageViewModel {
            get {
                return _previousPageViewModel;
            }
            set {
                _previousPageViewModel = value;
                RaisePropertyChanged("PreviousPageViewModel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
