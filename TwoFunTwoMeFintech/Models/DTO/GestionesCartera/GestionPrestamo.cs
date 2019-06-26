using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionPrestamo
    {
        public int IdCredito { get; set; }
        public string Identificacion { get; set; }
        public int IdSolicitud { get; set; }
        public string nombreProducto { get; set; }
        public decimal? MontoProducto { get; set; }
        public string FechaCredito { get; set; }
        public decimal? Interes_Corriente { get; set; }
        public decimal? InteresMora { get; set; }
        public decimal? MontoCuota { get; set; }
        public decimal? CapitalPendiente { get; set; }
    }
}