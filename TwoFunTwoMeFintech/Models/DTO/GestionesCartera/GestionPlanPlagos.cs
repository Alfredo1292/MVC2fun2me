using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionPlanPlagos
    {
        public int IdSolicitud { get; set; }
        public string FechaPago { get; set; }
        public string IdCredito { get; set; }
        public string FechaVencimiento { get; set; }
        public string MontoCuota { get; set; }
    }
}