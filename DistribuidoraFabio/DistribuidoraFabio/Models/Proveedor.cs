using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Proveedor
    {
        public int id_proveedor { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string contacto { get; set; }
        public int telefono { get; set; }
    }
}
