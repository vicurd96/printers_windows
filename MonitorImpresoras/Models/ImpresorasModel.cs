using System;
using System.Collections.Generic;
using System.Text;

namespace MonitorImpresoras.Models
{
    public class ImpresorasModel : BaseModel
    {
        private string _nombre, _puerto, _estado;
        private bool _compartida;
        private int _prioridad;
        public string Nombre { get => _nombre; set { _nombre = value; RaisePropertyChanged(nameof(Nombre)); } }
        public string Puerto { get => _puerto; set { _puerto = value; RaisePropertyChanged(nameof(Puerto)); } }
        public string Estado { get => _estado; set { _estado = value; RaisePropertyChanged(nameof(Estado)); } }
        public string Compartida { get => _compartida ? "Sí" : "No"; }
        public int Prioridad { get => _prioridad; set { _prioridad = value; RaisePropertyChanged(nameof(Prioridad)); } }
        public bool isCompartida { get => _compartida; set { _compartida = value; RaisePropertyChanged(nameof(isCompartida)); } }
    }
}
