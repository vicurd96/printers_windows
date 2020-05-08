using MonitorImpresoras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows.Data;

namespace MonitorImpresoras.ViewModels
{
    public class ImpresorasViewModel : ViewModelBase
    {
        public ObservableCollection<ImpresorasModel> ImpresorasModel { get; }
        public CollectionView CollectionView { get => (CollectionView)CollectionViewSource.GetDefaultView(ImpresorasModel); }

        public ImpresorasViewModel(LocalPrintServer local, PrintServer network) : base(local, network)
        {
            ImpresorasModel = new ObservableCollection<ImpresorasModel>();
            Inicializar();
        }

        public void Inicializar()
        {
            PrintQueueCollection queues = network.GetPrintQueues();

            foreach (PrintQueue queue in queues)
            {
                ImpresorasModel.Add(new ImpresorasModel { 
                    Nombre = queue.Name, 
                    isCompartida = queue.IsShared, 
                    Estado = queue.QueueStatus.ToString(), 
                    Puerto = queue.QueuePort.Name,
                    Prioridad = queue.Priority
                });
            }
            CollectionView.Refresh();
        }
    }
}
