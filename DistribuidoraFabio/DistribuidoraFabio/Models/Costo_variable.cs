using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Costo_variable
    {
        public int id_cv { get; set; }
        public string nombre_cv { get; set; }
        public decimal monto_cv { get; set; }
        public DateTime fecha_cv { get; set; }
        public int mes_cv { get; set; }
        public int gestion_cv { get; set; }
        public string descripcion_cv { get; set; }
        public string tipo_gasto_cv { get; set; }
    }
}
