using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
   public class Vendedores
    {

        public int id_vendedor { get; set; }
        public string nombre { get; set; }
        public int telefono { get; set; }
        public string direccion { get; set; }
        public string numero_cuenta { get; set; }
        public string cedula_identidad { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
    }
}
