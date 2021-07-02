using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class CantidadClientesVentas
	{
		public int cl_count { get; set; }
		public int? id_tipo_producto { get; set; }
		public int? id_vendedor { get; set; }
		public DateTime fecha_inicio { get; set; }
		public DateTime fecha_final { get; set; }
	}
}
