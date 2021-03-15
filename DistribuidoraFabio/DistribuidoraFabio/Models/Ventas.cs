using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Ventas
    {
        public int id_venta { get; set; }
        public DateTime fecha { get; set; }
        public int numero_factura { get; set; }
        public int id_cliente { get; set; }
        public int id_vendedor { get; set; }
        public string tipo_venta { get; set; }
        public decimal saldo { get; set; }
        public decimal total { get; set; }
        public DateTime fecha_entrega { get; set; }
        public string estado { get; set; }
        public string observacion { get; set; }
    }
}
