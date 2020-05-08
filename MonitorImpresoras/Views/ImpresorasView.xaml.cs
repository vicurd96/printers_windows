﻿using MonitorImpresoras.Models;
using MonitorImpresoras.ViewModels;
using System.Printing;
using System.Windows.Controls;

namespace MonitorImpresoras.Views
{
    /// <summary>
    /// Lógica de interacción para ImpresorasView.xaml
    /// </summary>
    public partial class ImpresorasView : UserControl
    {
        public ImpresorasView(LocalPrintServer local, PrintServer network)
        {
            InitializeComponent();
            DataContext = new ImpresorasViewModel(local, network);
        }
    }
}