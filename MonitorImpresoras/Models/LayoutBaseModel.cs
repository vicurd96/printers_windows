using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MonitorImpresoras.Models
{
    public interface IPageViewModel { }
    public class LayoutBaseModel : INotifyPropertyChanged
    {
        private IPageViewModel _previousPageViewModel, _currentPageViewModel;

        private List<IPageViewModel> _pageViewModels;
        public List<IPageViewModel> PageViewModels {
            get {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }

        }
        public IPageViewModel CurrentPageViewModel {
            get {
                return _currentPageViewModel;
            }
            set {
                PreviousPageViewModel = _currentPageViewModel;
                _currentPageViewModel = value;
                RaisePropertyChanged("CurrentPageViewModel");
            }
        }

        public IPageViewModel PreviousPageViewModel {
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
