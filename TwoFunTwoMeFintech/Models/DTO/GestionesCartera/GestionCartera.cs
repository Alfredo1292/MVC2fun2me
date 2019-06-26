using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionCartera
    {
        public string IdCredito { get; set; }
        public string IdAccion { get; set; }
        public string Descripcion { get; set; }
        public string EstadoGestion { get; set; }
        public bool Activo { get; set; }
        public string FechaGestion { get; set; }
        public string Detalle { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string RespuestaGestion { get; set; }
        public string MontoPromesaPago { get; set; }
        public string FechaPromesaPago { get; set; }
        public string FechaCobro { get; set; }
        public string FechaAplica { get; set; }
        public string TotalCobro { get; set; }
        public string Referencia { get; set; }
        public string CANTIDAD_PENDIENTES { get; set; }
        public string SALDO_PENDIENTE { get; set; }
        public string cod_agente { get; set; }
        public string total_promesas { get; set; }
        public string PROCESADOS { get; set; }
        public string Fecha { get; set; }
        public string TotalMora { get; set; }
        public string Dias_Mora { get; set; }

        public int IdGestion { get; set; }
        public string TipoGestion { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaAprovacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public string UsuarioAprobacion { get; set; }
    }
}