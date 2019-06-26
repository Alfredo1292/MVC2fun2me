using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionAccion
    {
        public int IdCredito { get; set; }
        public string Accion { get; set; }
        public string Mensaje { get; set; }
        public string Usuario { get; set; }
    }
}