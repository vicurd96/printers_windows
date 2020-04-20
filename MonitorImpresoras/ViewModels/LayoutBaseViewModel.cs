using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorImpresoras.ViewModels
{
    public class LayoutBaseViewModel
    {
        public LayoutBaseModel LayoutBaseModel { get; }

        public LayoutBaseViewModel() { LayoutBaseModel = new LayoutBaseModel(); }
        public LayoutBaseViewModel(List<IPageViewModel> pageViewModels)
        {
            LayoutBaseModel = new LayoutBaseModel();
            LayoutBaseModel.PageViewModels.AddRange(pageViewModels);
        }
        protected void ChangeViewModel(IPageViewModel viewModel)
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
