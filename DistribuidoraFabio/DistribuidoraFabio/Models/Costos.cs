using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class Costos
	{
        public int id_costos { get; set; }
        public string nombre_costos { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha { get; set; }
        public int mes { get; set; }
        public int gestion { get; set; }
        public string tipo_costo { get; set; }
        public string descripcion { get; set; }
    }
}
