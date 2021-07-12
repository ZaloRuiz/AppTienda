using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class ReporteVentaDiaria
	{
		public DateTime fecha_inicio { get; set; }
		public DateTime fecha_final { get; set; }
		public string nombre_producto { get; set; }
		public decimal monto_vend { get; set; }
		public int cantidad { get; set; }
	}
}
