using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class PasosXAgente
    {
        public int IdSolicitud { get; set; }
        public string CodAgente { get; set; }
        public string Paso { get; set; }
        public string Descripcion { get; set; }
        public Boolean BitCompleto { get; set; }
        public string Url { get; set; }
        public int IdTipo { get; set; }
    }
}