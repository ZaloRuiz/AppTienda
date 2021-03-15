using System;
using System.Collections.Generic;
using System.Text;


namespace DistribuidoraFabio.Models
{
    public class Cliente
    {        
        public int id_cliente { get; set; }
        public int codigo_c { get; set; }
        public string nombre_cliente { get; set; }
	    public string direccion_cliente { get; set; }
        public string ubicacion_latitud { get; set; }
        public string ubicacion_longitud { get; set; }
        public int telefono { get; set; }
        public string razon_social { get; set; }
        public int nit { get; set; }
    }
}
