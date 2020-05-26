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
        protected PrintServer servidor;

        public LayoutBaseViewModel(PrintServer servidor) 
        { 
            LayoutBaseModel = new LayoutBaseModel(); 
            this.servidor = servidor;
        }
        public LayoutBaseViewModel(List<UserControl> pageViewModels, PrintServer servidor)
        {
            LayoutBaseModel = new LayoutBaseModel();
            LayoutBaseModel.PageViewModels.AddRange(pageViewModels);
            this.servidor = servidor;
        }
    }
}
