using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class Correo
    {
        public int IdSolicitud { get; set; }
        public string Mail { get; set; }
        public string URL { get; set; }
        public string valid { get; set; }
        public string message { get; set; }
    }
}