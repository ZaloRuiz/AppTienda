using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
	public class ProductoNombre
	{
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public string nombre_tipo_producto { get; set; }
        public decimal stock { get; set; }
        public decimal stock_valorado { get; set; }
        public decimal promedio { get; set; }
        public decimal precio_venta { get; set; }
        public decimal producto_alerta { get; set; }
        public string display_text_nombre { get { return $"{nombre_producto} {nombre_tipo_producto}"; } }
    }
}
