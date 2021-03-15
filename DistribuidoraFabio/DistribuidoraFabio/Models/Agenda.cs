using System;
using System.Collections.Generic;
using System.Text;

namespace DistribuidoraFabio.Models
{
    public class Agenda
    {
        public int id_agenda { get; set; }
        public string titulo { get; set; }
        public DateTime fecha { get; set; }
        public string hora { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
    }
}
