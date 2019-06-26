using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class DTOHistorialCobros
    {
        public string Identificacion { get; set; }
        public int IdCredito { get; set; }
        public string Nombre { get; set; }
        public string Banco { get; set; }
        public string Referencia { get; set; }
        public decimal TotalCobro { get; set; }
        public string FechaCobro { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
    }
}