using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MonitorImpresoras.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        protected Dictionary<int, string> jobStatusDict = new Dictionary<int, string>()
                                {
                                    {0, "Desconocido"},
                                    {1, "Pausado"},
                                    {2, "Error"},
                                    {4, "Eliminando"},
                                    {8, "Enviando"},
                                    {16, "Imprimiendo"},
                                    {32, "Offline"},
                                    {64, "Sin papel"},
                                    {128, "Impreso"},
                                    {256, "Eliminado"},
                                    {512, "Bloqueado"},
                                    {1024, "Interrumpido por usuario"},
                                    {2048, "Reiniciado"},
                                    {4096, "Completado"},
                                    {8192, "Retenido"},
                                };
        protected Dictionary<int, string> jobPriorityDict = new Dictionary<int, string>
        {
            { 0, "0 (N/A)" },
            { 1, "1 (Por defecto)" },
            { 99, "99 (Máxima)" }
        };
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
