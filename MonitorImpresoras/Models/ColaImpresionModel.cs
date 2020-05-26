using Atom8.API.PrintSpool;
using System.Printing;

namespace MonitorImpresoras.Models
{
    public class ColaImpresionModel : BaseModel
    {
        private int _id;
        private string _name;
        private JOBSTATUS _status;
        public int Id { get => _id; set { _id = value; RaisePropertyChanged("Id"); } }
        public string Name { get => _name; set { _name = value; RaisePropertyChanged(nameof(Name)); } }
        public JOBSTATUS Status { get => _status; set { _status = value; RaisePropertyChanged("Status"); } }
    }
}
