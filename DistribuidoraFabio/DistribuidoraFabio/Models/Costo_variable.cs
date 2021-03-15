using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Costo_variable
    {
        public int id_costo_variable { get; set; }
        public string nombre { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha { get; set; }
        public int mes { get; set; }
        public int gestion { get; set; }
        public string descripcion { get; set; }
    }
}
