using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class DetalleCompraNombre
	{
        public int id_dc { get; set; }
        public int numero_factura { get; set; }
        public string nombre_producto { get; set; }
        public int cantidad_compra { get; set; }
        public decimal precio_producto { get; set; }
        public decimal descuento_producto { get; set; }
        public decimal sub_total { get; set; }
    }
}
