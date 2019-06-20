using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Models
{
    public class dto_login
    {
        [Key]
        [DisplayName("Agente")]
        public string cod_agente { get; set; }

        [DisplayName("Nombre Cliente:")]
        public string nombre { get; set; }

        public string Mensaje { get; set; }

        public string pass { get; set; }

        public string vendedor { get; set; }

        public DateTime? fecha_cambiopassword { get; set; }

        public string STR_USUARIO_AD { get; set; }

        public string STR_COD_AGENTE_PADRE { get; set; }

        public bool? BIT_DISPONIBLE { get; set; }

        public int? INT_COD_AREA { get; set; }

        public DateTime? FEC_CREACION { get; set; }

        public DateTime? FEC_MODIFICACION { get; set; }

        public string estado { get; set; }

        [Display(Name = "Correo"), Required]
        public string correo { get; set; }

        [Display(Name = "Rol"), Required]
        public int? ROLID { get; set; }
		public int? IdCatDisponibilidad { get; set; }
		public string ROLES { get; set; }
        public int ConfirmPassword { get; set; }
        public string ConfiguracionBucket { get; set; }
        public List<dto_login> listadoDto_login { get; set; }

        public List<Roles> ListRoles { get; set; }
        public List<UserAgencias>ListaAgencias { get; set; }
        public bool esTemporal { get; set; }
        public string Extencion { get; set; }

		public string Respuesta { get; set; }

		[Display(Name = "Asignacion"), Required]
        public int? TipoCola { get; set; }
        [Display(Name = "Agencia"), Required]
        public int? IdAgencia { get; set; }
        public string NombreAgencia { get; set; }
    }





    public class dto_AsignacionBuckets
    {
        public int id { get; set; }

        public int BucketInicial { get; set; }

        public int BucketFinal { get; set; }

        public bool? PRPRotas { get; set; }

        public bool? CeroPagos { get; set; }

        public bool? SaldoAlto { get; set; }

        public bool? SaldoMedio { get; set; }

        public bool? SaldoBajo { get; set; }

        public bool? Estado { get; set; }

        public bool? CuentaAlDia { get; set; }


    }

    public class dto_CantidadBucketsNoAsignado
    {
        public int Cantidad_Cuentas { get; set; }

        public String Bucket { get; set; }

        public bool? PromesaRota { get; set; }

        public bool? CeroPagos { get; set; }

        public bool? RangoSaldoAlto { get; set; }

        public bool? RangoSaldoMedio { get; set; }

        public bool? RangoSaldoBajo { get; set; }

        public bool? CuentaAlDia { get; set; }


    }

    public class dto_CantidadBuckets
    {
        public int Cantidad_Cuentas { get; set; }

        public string AgenteAsignado { get; set; }

        public String Bucket { get; set; }

        public bool? PromesaRota { get; set; }

        public bool? CeroPagos { get; set; }

        public bool? RangoSaldoAlto { get; set; }

        public bool? RangoSaldoMedio { get; set; }

        public bool? RangoSaldoBajo { get; set; }

        public bool? Estado { get; set; }

        public bool? CuentaAlDia { get; set; }


    }


    public class dto_Configuracion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}