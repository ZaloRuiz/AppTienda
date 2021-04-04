using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class VentasPorCliente
	{
		public DateTime fecha_inicio { get; set; }
		public DateTime fecha_final { get; set; }
		public int cliente_count { get; set; }
		public decimal monto_vend { get; set; }
		public string nombre_cliente { get; set; }
	}
}
