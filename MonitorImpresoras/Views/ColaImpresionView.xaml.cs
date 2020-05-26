using MonitorImpresoras.ViewModels;
using System.Printing;
using System.Windows.Controls;

namespace MonitorImpresoras.Views
{
    /// <summary>
    /// Lógica de interacción para ColaImpresionView.xaml
    /// </summary>
    public partial class ColaImpresionView : UserControl
    {
        public ColaImpresionView(PrintServer servidor)
        {
            InitializeComponent();
            DataContext = new ColaImpresionViewModel(servidor);
        }
    }
}
