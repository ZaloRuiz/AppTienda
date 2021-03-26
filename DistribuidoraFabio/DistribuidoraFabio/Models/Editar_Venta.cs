using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Editar_Venta
    {
        public int id_ed_venta { get; set; }
        public int id_venta { get; set; }
        public int id_vendedor { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
    }
}
