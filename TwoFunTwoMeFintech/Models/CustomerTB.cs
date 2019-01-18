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
    }


    

}
