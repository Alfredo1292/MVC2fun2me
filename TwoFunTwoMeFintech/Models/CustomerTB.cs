using System;
using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models
{
    public class agente
    {
        [Key]
        public string cod_agente { get; set; }
        public string nombre { get; set; }
        public string ConfiguracionBucket { get; set; }
        private DateTime FEC_CREACION { get; set; }
        public DateTime FEC_MODIFICACION { get; set; }
        public string estado { get; set; }
        public string Extencion { get; set; }
        public int IdCredito { get; set; }
        public string IdentificacionCliente { get; set; }
        public string TelefonoCelCliente { get; set; }
        
    }
}
