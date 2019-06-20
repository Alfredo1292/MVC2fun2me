using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TwoFunTwoMeFintech.Models.DTO
{
	public class CuentaBancaria
	{
		public int Id { get; set; }

		public string Identificacion { get; set; }

		public string DescCuenta { get; set; }

		public int? IdPersona { get; set; }

		public int? IdBanco { get; set; }
	
		public string Descripcion { get; set; }

		public string DescMoneda { get; set; }

		public string Cuenta { get; set; }

		public string CuentaSinpe { get; set; }

		public string CuentaIban { get; set; }

		public DateTime? FechaIngreso { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public string UsrModifica { get; set; }

		public bool? Predeterminado { get; set; }

		public string Operaciones { get; set; }

		public string DescPredet { get; set; }

		public int? IdTipoMoneda { get; set; }

		public int? IdTipoCuenta { get; set; }

		public string Respuesta { get; set; }

		public bool result { get; set; }
		public List<Bancos> ListBancos { get; set; }

		public List<TipoMoneda> ListTipoMoneda { get; set; }

		public List<TipoCuentas> ListTipoCuentas { get; set; }

        //INICIO FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
        public int IdSolicitud { get; set; }
        //FIN FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD

        public int Verificacion { get; set; }
        public string VerificacionStatus { get; set; }
    }

}