using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class _RDetalleVenta
    {
        public int id_venta { get; set; }
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public int codigo_c { get; set; }
        public string nombre_cliente { get; set; }
        public string razon_social { get; set; }
        public int nit { get; set; }
        public int telefono { get; set; }
        public string direccion_cliente { get; set; }
        public string geolocalizacion { get; set; }
        public string nombre_producto { get; set; }
        public decimal precio_producto { get; set; }
        public int cantidad { get; set; }
        public decimal sub_total { get; set; }
        public int envases { get; set; }
        public string tipo_venta { get; set; }
        public string estado { get; set; }
    }
}
