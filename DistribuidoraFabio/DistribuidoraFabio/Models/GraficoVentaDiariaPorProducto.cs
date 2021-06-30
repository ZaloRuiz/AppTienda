using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class GraficoVentaDiariaPorProducto
    {
        public int id_dv { get; set; }
        public DateTime fecha { get; set; }
        public int id_vendedor { get; set; }
        public string nombre_vendedor { get; set; }
        public decimal sub_total { get; set; }
        public int id_tipo_producto { get; set; }
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public string estado { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_final { get; set; }
    }
}
