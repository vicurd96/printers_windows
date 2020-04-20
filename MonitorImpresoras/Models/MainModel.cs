using System;
using System.Collections.Generic;
using System.Text;

namespace MonitorImpresoras.Models
{
    public class MainModel : BaseModel
    {
        private string _texto;
        public string Texto { get => _texto; set { _texto = value; RaisePropertyChanged(nameof(Texto)); } }
    }
}
