using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class TiempoSolicitud
    {
        public int IdSolicitud { get; set; }
        public string UsrModifica { get; set; }
        public int Tiempo { get; set; }
        public string Origen { get; set; }
    }
}