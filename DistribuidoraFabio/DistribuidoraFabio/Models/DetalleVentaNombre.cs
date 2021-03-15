using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class DetalleVentaNombre
	{
        public int id_dv { get; set; }
        public int cantidad { get; set; }
        public string nombre_producto { get; set; }
        public string nombre_sub_producto { get; set; }
        public decimal precio_producto { get; set; }
        public decimal descuento { get; set; }
        public decimal sub_total { get; set; }
        public int envases { get; set; }
        public int factura { get; set; }
        public string display_text_nombre { get { return $"{nombre_producto} {nombre_sub_producto}"; } }
    }
}
