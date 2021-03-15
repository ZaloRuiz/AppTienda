using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Compras
    {
        public int id_compra{ get; set; }
        public DateTime fecha_compra { get; set; }
        public int numero_factura { get; set; }
        public int id_proveedor { get; set; }
        public decimal saldo { get; set; }
        public decimal total { get; set; }        
    }
}
