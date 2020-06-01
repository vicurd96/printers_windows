using Atom8.API.PrintSpool;
using System.Printing;

namespace MonitorImpresoras.Models
{
    public class TrabajoImpresionModel : BaseModel
    {
        private int _id, _numPages;
        private string _name, _owner, _estado, _prioridad, _printerName;
        private JOBSTATUS _status;
        private int _priority;

        public int Id { get => _id; set { _id = value; RaisePropertyChanged("Id"); } }
        public string Name { get => _name; set { _name = value; RaisePropertyChanged(nameof(Name)); } }
        public JOBSTATUS JobStatus { get => _status; set { _status = value; RaisePropertyChanged("Status"); } }
        public string Estado { get => jobStatusDict.TryGetValue((int)_status, out _estado) ? _estado : ""; set { _estado = value; RaisePropertyChanged(nameof(Estado)); } }
        public int NumPages { get => _numPages; set { _numPages = value; RaisePropertyChanged("NumPages"); } }
        public string Owner { get => _owner; set { _owner = value; RaisePropertyChanged(nameof(Owner)); } }
        public string Priority { get => jobPriorityDict.TryGetValue(_priority, out _prioridad) ? _prioridad : _priority.ToString(); set { if(int.TryParse(value, out _priority)) RaisePropertyChanged(nameof(Priority)); } }
        public string PrinterName { get => _printerName; set { _printerName = value; RaisePropertyChanged(nameof(PrinterName)); } }
    }
}
