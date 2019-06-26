using System;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class CobrosPendientes
    {
        public string Filtro { get; set; }
        public string Credito { get; set; }
        public string NombrePersona { get; set; }
        public string Identificacion { get; set; }
        public string FechaTransferencia { get; set; }
        public Decimal MontoCuota { get; set; }
        public Decimal CapitalPendiente { get; set; }
    }
}