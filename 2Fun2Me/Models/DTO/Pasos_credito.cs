using System;

namespace TwoFunTwoMe.Models.DTO
{
    public class Pasos_credito
    {
		public int? IdPasosCredito { get; set; }
		public string IdTipoIdentificacion { get; set; }

		public string Identificacion { get; set; }

		public string mobile { get; set; }
		public string phone { get; set; }
		public string tablet { get; set; }
		public string UsrAgent { get; set; }
		public string os { get; set; }
		public int? isIphone { get; set; }
		public string webbit { get; set; }
		public string systemBuild { get; set; }
		public string publicIpAddress { get; set; }

		public int? IdSolicitud { get; set; }

		public bool? Paso_1 { get; set; }

		public string Telefono_celuar { get; set; }

		public string Correo { get; set; }

		public int? IdProducto { get; set; }

		public bool? Paso_2 { get; set; }

		public string Pin { get; set; }

		public bool? Paso_3 { get; set; }

		public bool? Video { get; set; }

		public bool? Paso_4 { get; set; }

		public bool? Credito_Aprobado { get; set; }

		public bool? Paso_5 { get; set; }

		public string Foto_identificacion { get; set; }

		public bool? Paso_6 { get; set; }

		public string Foto_identificacion_Trasera { get; set; }

		public bool? Paso_7 { get; set; }

		public string Selfie_identificacion { get; set; }

		public bool? Paso_8 { get; set; }

		public string Cuenta_cliente { get; set; }

		public int? Banco { get; set; }

		public bool? Paso_9 { get; set; }

		public bool? ConsultaDatosContratoPagare { get; set; }

		public bool? Paso_10 { get; set; }

		public string Firma { get; set; }

		public bool? Paso_11 { get; set; }

		public bool? GeneraContratoPagare { get; set; }

		public bool? Paso_12 { get; set; }

		public string Empresa { get; set; }

		public bool? Paso_13 { get; set; }

		public bool? PinVerificado { get; set; }

		public DateTime? FECHA_INSERTA { get; set; }

		public DateTime? FECHA_MODIFICA { get; set; }

		public string Mensaje { get; set; }
	   

	}
}