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
        PrintServer network = new PrintServer();
        LocalPrintServer local = new LocalPrintServer();
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel(local, network);
        }
    }
}
