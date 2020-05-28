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
                                    {0, "Sin especificar"},
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
        protected Dictionary<int, string> printQueueStatus = new Dictionary<int, string>()
                                {
                                    {0, "Sin especificar"},
                                    {1, "Pausado"},
                                    {2, "Error al imprimir"},
                                    {4, "Eliminando"},
                                    {8, "Papel atascado"},
                                    {16, "Impresora sin papel"},
                                    {32, "Pendiente bandeja manual"},
                                    {64, "Papel causando error no esperado"},
                                    {128, "Sin conexión"},
                                    {256, "Intercambiando datos"},
                                    {512, "Ocupado"},
                                    {1024, "Imprimiendo"},
                                    {2048, "La bandeja de salida está llena"},
                                    {4096, "Información de estado no disponible"},
                                    {8192, "Esperando trabajo de impresión"},
                                    {16384, "Procesando algún tipo de trabajo"},
                                    {32768, "Inicializando"},
                                    {65536, "La impresora se está calentando"},
                                    {131072, "Poco tóner"},
                                    {262144, "Sin toner"},
                                    {524288, "No se puede imprimir la página actual"},
                                    {1048576, "Se requiere la acción del usuario para corregir error"},
                                    {2097152, "Sin memoria disponible"},
                                    {4194304, "Hay una puerta abierta en la impresora"},
                                    {8388608, "La impresora está en un estado de error"},
                                    {16777216, "La impresora está en modo de ahorro de energía"}
                                };
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
