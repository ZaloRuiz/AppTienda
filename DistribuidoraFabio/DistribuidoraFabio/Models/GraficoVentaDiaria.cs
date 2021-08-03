using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class GraficoVentaDiaria
    {
        public int id_venta { get; set; }
        public DateTime fecha { get; set; }
        public int id_vendedor { get; set; }
        public string nombre_vendedor { get; set; }
        public decimal total { get; set; }
        public string estado { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_final { get; set; }
    }
}
