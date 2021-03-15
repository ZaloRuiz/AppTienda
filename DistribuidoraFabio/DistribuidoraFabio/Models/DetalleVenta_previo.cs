using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class DetalleVenta_previo
	{
        public int cantidad { get; set; }
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public decimal precio_producto { get; set; }
        public decimal descuento { get; set; }
        public int envases { get; set; }
        public decimal sub_total { get; set; }
        public decimal stock { get; set; }
        public decimal stock_valorado { get; set; }
        public decimal promedio { get; set; }
    }
}
