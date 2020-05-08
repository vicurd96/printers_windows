using MonitorImpresoras.Models;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Windows.Controls;

namespace MonitorImpresoras.ViewModels
{
    public class LayoutBaseViewModel
    {
        public LayoutBaseModel LayoutBaseModel { get; }
        protected PrintServer network;
        protected LocalPrintServer local;

        public LayoutBaseViewModel(LocalPrintServer local, PrintServer network) 
        { 
            LayoutBaseModel = new LayoutBaseModel(); 
            this.network = network;
            this.local = local;
        }
        public LayoutBaseViewModel(List<UserControl> pageViewModels, LocalPrintServer local, PrintServer network)
        {
            LayoutBaseModel = new LayoutBaseModel();
            LayoutBaseModel.PageViewModels.AddRange(pageViewModels);
            this.network = network;
            this.local = local;
        }
        protected void ChangeViewModel(UserControl viewModel)
        {
            if (!LayoutBaseModel.PageViewModels.Contains(viewModel))
                LayoutBaseModel.PageViewModels.Add(viewModel);

            LayoutBaseModel.CurrentPageViewModel = LayoutBaseModel.PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        protected void GoPreviousViewModel(object obj)
        {
            ChangeViewModel(LayoutBaseModel.PreviousPageViewModel);
        }
    }
}
