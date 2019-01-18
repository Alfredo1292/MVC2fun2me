using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class AsignacionBuckets
    {
        [Key]
        public int id { get; set; }

        public int BucketInicial { get; set; }

        public int BucketFinal { get; set; }


        public bool PRPRotas { get; set; }


        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Cero Pagos:"), Required]
        public bool CeroPagos { get; set; }

        [Display(Name = "Saldo Alto:"), Required]
        public bool SaldoAlto { get; set; }

        [Display(Name = "Saldo Medio:"), Required]
        public bool SaldoMedio { get; set; }

        [Display(Name = "Saldo Bajo:"), Required]
        public bool SaldoBajo { get; set; }

        [Display(Name = "Estado:"), Required]
        public bool Estado { get; set; }

        [Display(Name = "CuentaAlDia:"), Required]
        public bool CuentaAlDia { get; set; }

        [DataMember]
        public List<AsignacionBuckets> listadoBucket { get; set; }

        [Display(Name = "Mensaje:")]
        public string Mensaje { get; set; }

        [Display(Name = "Mensaje de Error:")]
        public string MensajeError { get; set; }

        public bool Respuesta { get; set; }

    }
}