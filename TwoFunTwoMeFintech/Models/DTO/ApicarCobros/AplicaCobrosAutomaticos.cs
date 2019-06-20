using System;

namespace TwoFunTwoMeFintech.Models.DTO.ApicarCobros
{
    public class AplicaCobrosAutomaticos
    {
        public string Action { get; set; }
        public long Id { get; set; }

        public long? IdTipoCobrosAutomaticos { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string Usuario { get; set; }
    }
}