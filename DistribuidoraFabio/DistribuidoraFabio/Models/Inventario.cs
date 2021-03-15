using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Inventario
    {
        public int id_inventario { get; set; }
        public int id_producto { get; set; }
        public DateTime fecha_inv { get; set; }
        public int numero_factura { get; set; }
        public string detalle { get; set; }
        public decimal precio_compra { get; set; }
        public decimal unidades { get; set; }
        public decimal entrada_fisica { get; set; }
        public decimal salida_fisica { get; set; }
        public decimal saldo_fisica { get; set; }
        public decimal entrada_valorado { get; set; }
        public decimal salida_valorado { get; set; }
        public decimal saldo_valorado { get; set; }
        public decimal promedio { get; set; }
    }
}
