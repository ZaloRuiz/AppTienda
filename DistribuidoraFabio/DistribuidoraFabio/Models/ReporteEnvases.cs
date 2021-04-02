using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class ReporteEnvases
	{
        public int id_dv { get; set; }
        public int cantidad { get; set; }
        public string nombre_producto { get; set; }
        public decimal precio_producto { get; set; }
        public decimal descuento { get; set; }
        public decimal sub_total { get; set; }
        public int envases { get; set; }
        public int factura { get; set; }
        public DateTime fecha { get; set; }
        public string nombre_cliente { get; set; }
        public string nombre_vendedor { get; set; }
    }
}
