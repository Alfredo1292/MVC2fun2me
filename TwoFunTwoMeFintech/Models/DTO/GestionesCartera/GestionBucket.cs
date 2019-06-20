using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionBucket
    {
        [Key]
        public int IdCredito { get; set; }
        public int? DiasMora { get; set; }
        public string Nombre { get; set; }
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
        public int? Bandera { get; set; }
        public int? BucketNuevo { get; set; }
        public string AgenteAsignado { get; set; }
        public string ResultadoSig { get; set; }
        public int? Asignado { get; set; }
        public bool? Procesado { get; set; }
        public string cod_agente { get; set; }
        public string Identificacion { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
    }
}