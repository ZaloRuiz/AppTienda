using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class DetalleVenta
    {
        public int id_dv { get; set; }
        public int cantidad { get; set; }
        public int id_producto { get; set; }
        public decimal precio_producto { get; set; }
        public decimal descuento { get; set; }
        public decimal sub_total { get; set; }
        public int envases { get; set; }
        public int factura { get; set; }
    }
}
