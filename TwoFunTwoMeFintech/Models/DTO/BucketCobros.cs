using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class BucketCobros
    {
        [Key]
        public int IdCredito { get; set; }

        public int? DiasMora { get; set; }

        public decimal? TotalMora { get; set; }

        public int? IdPersona { get; set; }

        public int? IdSolicitud { get; set; }

        public bool? PromesaRota { get; set; }

        public bool? CeroPagos { get; set; }

        public bool? CuentaAlDia { get; set; }

        public int? RangoSaldoBajo { get; set; }

        public int? RangoSaldoMedio { get; set; }

        public int? RangoSaldoAlto { get; set; }

        public int? Bucket { get; set; }

        public int? Estado { get; set; }

        public int? BucketNuevo { get; set; }

        public string AgenteAsignado { get; set; }

        public int? Asignado { get; set; }

        public bool? Procesado { get; set; }

        public string cod_agente { get; set; }

        public string Identificacion { get; set; }
       
    }

}