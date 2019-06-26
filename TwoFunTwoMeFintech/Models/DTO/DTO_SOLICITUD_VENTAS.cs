using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TwoFunTwoMeFintech.Models.DTO
{

	public struct Pasos_Credito
	{
		#region PASOS DE CREDITO TRACE

		public string Paso { get; set; }
		public string Columna { get; set; }

		public string Dato { get; set; }
		public string Version { get; set; }

		public string Paso_1 { get; set; }

		public string IdSolicitud { get; set; }

		public string Identificacion { get; set; }

		public string Telefono_celuar { get; set; }

		public string Correo { get; set; }

		public string idProducto { get; set; }

		public string Paso_2 { get; set; }

		public string pin { get; set; }

		public string Paso_3 { get; set; }

		public string Video { get; set; }

		public string Paso_4 { get; set; }

		public string Credito_Aprobado { get; set; }

		public string Paso_5 { get; set; }

		public string Foto_identificacion { get; set; }

		public string Paso_6 { get; set; }

		public string Foto_identificacion_Trasera { get; set; }

		public string Paso_7 { get; set; }

		public string Selfie_identificacion { get; set; }

		public string Paso_8 { get; set; }

		public string Cuenta_cliente { get; set; }

		public string Paso_9 { get; set; }

		public string ConsultaDatosContratoPagare { get; set; }

		public string Paso_10 { get; set; }

		public string Firma { get; set; }

		public string Paso_11 { get; set; }

		public string GeneraContratoPagare { get; set; }

		public string Paso_12 { get; set; }

		public string Empresa { get; set; }

		#endregion

	}

	public class DTO_SOLICITUD_VENTAS
	{

		[DataMember]
		public List<DTO_SOLICITUD_VENTAS> ListSolicitud { get; set; }

		public string Identificacion { set; get; }
		public int? IdSolicitud { get; set; }
		public string Solicitud { get; set; }
        public int tipo { get; set; }
        public string MontoProducto { get; set; }
		//Agrego que permita nulos //Alfredo Jose Vargas Seinfarth- INICIO
        public int? mostrarimagenes { get; set; }
		//Agrego que permita nulos //Alfredo Jose Vargas Seinfarth- Final
		public decimal? MontoMaximo { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
		public string Telefono { get; set; }
		public string Mail { get; set; }
		public string EstadoDoc { get; set; }
		public string Usuario { get; set; }
		public string EstadoDeProcesoSol { get; set; }
		public string UrlFotoCedula { get; set; }
		public string UrlFotoSelfie { get; set; }
		public string UrlFotoCedulaTrasera { set; get; }
		public string UrlDirectorioPagare { set; get; }
		public string UrlFotoFirma { set; get; }
		public string UrlFoto { get; set; }
		public int? TipoFoto { get; set; }
		public string PorcentMatched { set; get; }
		public string UnMatchedFace { set; get; }
		public string Estado { set; get; }
		public int? Status { set; get; }
        public string StatusDescripcion { set; get; }
        public string Respuesta { set; get; }
		public string Mensaje { set; get; }
		public string MensajeError { set; get; }
		public string USUARIO_MODIFICACION { set; get; }
		public string Descripcion { set; get; }
        public string SubOrigen { get; set; }

		//PARAM usp_ListarSolicXAsesor
		public string FiltroAsesor { get; set; }

		public List<Pasos_Credito> _PasosCredito;

		public List<Tipos> ListTipos { get; set; }

		public string MuestraPasosCredito;

		public int IdMontoCredito { get; set; }

		public int IdPlazoCredito { get; set; }

		public int IdFrecuencia { get; set; }
	}


}
