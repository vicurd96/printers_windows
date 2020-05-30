using MonitorImpresoras.ViewModels;
using System.Printing;
using System.Windows;

namespace MonitorImpresoras
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        PrintServer servidor = new LocalPrintServer();
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel(servidor);
        }
    }
}
