using System;
using System.Collections.Generic;
using System.Text;

namespace MonitorImpresoras.Helpers
{
    public enum Accion
    {
        SubirPrioridad,
        BajarPrioridad,
        Pausar,
        Reanudar,
        Reiniciar
    }

    public enum Metodo
    {
        ActualizarJobs,
        CambiarPrioridadJob,
        CancelarJob,
        CambiarStatusJob,
        CambiarStatusPrinter
    }

    public static class Mediator
    {
        private static IDictionary<Metodo, List<Action<object>>> pl_dict =
           new Dictionary<Metodo, List<Action<object>>>();

        public static void Subscribe(Metodo token, Action<object> callback)
        {
            if (!pl_dict.ContainsKey(token))
            {
                var list = new List<Action<object>>();
                list.Add(callback);
                pl_dict.Add(token, list);
            }
            else
            {
                bool found = false;
                foreach (var item in pl_dict[token])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                if (!found)
                    pl_dict[token].Add(callback);
            }
        }

        public static void Unsubscribe(Metodo token, Action<object> callback)
        {
            if (pl_dict.ContainsKey(token))
                pl_dict[token].Remove(callback);
        }

        public static void Notify(Metodo token, object args = null)
        {
            if (pl_dict.ContainsKey(token))
                foreach (var callback in pl_dict[token])
                    callback(args);
        }
    }
}
