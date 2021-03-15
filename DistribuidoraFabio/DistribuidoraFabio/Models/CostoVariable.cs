using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class CostoVariable
    {
        public string nombre { get; set; }
        public decimal monto_enero { get; set; }
        public decimal monto_febrero { get; set; }
        public decimal monto_marzo { get; set; }
        public decimal monto_abril { get; set; }
        public decimal monto_mayo { get; set; }
        public decimal monto_junio { get; set; }
        public decimal monto_julio { get; set; }
        public decimal monto_agosto { get; set; }
        public decimal monto_septiembre { get; set; }
        public decimal monto_octubre { get; set; }
        public decimal monto_noviembre { get; set; }
        public decimal monto_diciembre { get; set; }
        public decimal DisplayMontoTotal => monto_enero + monto_febrero + monto_marzo + monto_abril + monto_mayo + monto_junio
                                          + monto_julio + monto_agosto + monto_septiembre + monto_octubre + monto_noviembre + monto_diciembre;
    }
}
