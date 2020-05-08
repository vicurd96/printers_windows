using MonitorImpresoras.ViewModels;
using System.Printing;
using System.Windows.Controls;

namespace MonitorImpresoras.Views
{
    /// <summary>
    /// Lógica de interacción para ProcesosImpresionView.xaml
    /// </summary>
    public partial class ProcesosImpresionView : UserControl
    {
        public ProcesosImpresionView(LocalPrintServer local, PrintServer network)
        {
            InitializeComponent();
            DataContext = new ProcesosImpresionViewModel(local, network);
        }
    }
}
