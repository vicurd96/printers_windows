using System;
using System.Collections.Generic;
using System.Text;

namespace MonitorImpresoras.Models
{
    public class ImpresorasModel : BaseModel
    {
        private string _nombre, _puerto, _estado;
        private int _status;
        private bool _compartida;
        private int _prioridad;
        public string Nombre { get => _nombre; set { _nombre = value; RaisePropertyChanged(nameof(Nombre)); } }
        public string Puerto { get => _puerto; set { _puerto = value; RaisePropertyChanged(nameof(Puerto)); } }
        public int Status { get => _status; set { _status = value; RaisePropertyChanged("Status"); } }
        public string Estado { get => printQueueStatus.TryGetValue(_status, out _estado) ? _estado : ""; }
        public string Compartida { get => _compartida ? "Sí" : "No"; }
        public int Prioridad { get => _prioridad; set { _prioridad = value; RaisePropertyChanged(nameof(Prioridad)); } }
        public bool isCompartida { get => _compartida; set { _compartida = value; RaisePropertyChanged(nameof(isCompartida)); } }
        public bool isRed { get => _red; set { _red = value; RaisePropertyChanged(nameof(isRed)); } }
    }
}
