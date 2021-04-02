using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class CostoFijo_Query
    {
        public decimal monto_cf { get; set; }
        public DateTime fecha_cf { get; set; }
        public int mes_cf { get; set; }
        public int gestion_cf { get; set; }
        public decimal DisplayMontoTotal => monto_cf * 12;
    }
}
