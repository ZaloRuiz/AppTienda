using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Xamarin.Forms;

namespace DistribuidoraFabio.Models
{
    public class Producto
    {        
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public int id_tipo_producto { get; set; }
        public decimal stock { get; set; }
        public decimal stock_valorado { get; set; }
        public decimal promedio { get; set; }
        public decimal precio_venta { get; set; }
        public decimal producto_alerta { get; set; }
    }
}