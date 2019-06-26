using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionArchivoSubir
    {
        public string Base64 { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public string Identificacion { set; get; }
    }
}