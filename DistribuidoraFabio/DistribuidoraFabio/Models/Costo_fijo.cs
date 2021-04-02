﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Costo_fijo
    {
        public int id_cf { get; set; }
        public string nombre_cf { get; set; }
        public decimal monto_cf { get; set; }
        public DateTime fecha_cf { get; set; }
        public int mes_cf { get; set; }
        public int gestion_cf { get; set; }
        public string descripcion_cf { get; set; }
        public string tipo_gasto_cf { get; set; }
        public decimal DisplayMontoTotal => monto_cf * 12;
        public string DisplayFecha { get { return $"{mes_cf} / {gestion_cf}"; }
}
    }
}
