using MonitorImpresoras.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonitorImpresoras.Views
{
    /// <summary>
    /// Lógica de interacción para ColaImpresionView.xaml
    /// </summary>
    public partial class ColaImpresionView : UserControl
    {
        public ColaImpresionView()
        {
            InitializeComponent();
            DataContext = new ColaImpresionViewModel();
        }
    }
}
