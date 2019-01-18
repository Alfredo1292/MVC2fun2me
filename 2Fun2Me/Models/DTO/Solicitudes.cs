using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TwoFunTwoMe.Models.DTO
{
    public class Solicitudes
    {
        [Key]
        public int? IdSolicitud { get; set; }
        [Required]
        public string IdTipoIdentificacion { set; get; }
        [Required]
        public string Identificacion { set; get; }
        public int? IdBuro { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsrModifica { get; set; }

        public int? IdPersona { get; set; }

        public int? Status { get; set; }

        public DateTime? FechaTransferencia { get; set; }

        public string Comentarios { get; set; }

        public string UsoCredito { get; set; }

        public bool? Consentimiento { get; set; }

        public int? IdProducto { get; set; }

        public int? IdAgente { get; set; }

        public int? SoporteTipo { get; set; }

        public int? IdStatusOrden { get; set; }

        public bool? FotoCedula { get; set; }

        public bool? Recibo { get; set; }

        public bool? Contrato { get; set; }

        public bool? Pagare { get; set; }

        public bool? ReciboIngresos { get; set; }

        public bool? CuentaBancaria { get; set; }

        public bool? Selfie { get; set; }

        public string Origen { get; set; }

        public int? IdUsrAsig { get; set; }

        public int? IdEstadoProc { get; set; }

        public int? SubStatus { get; set; }

        public decimal? MontoMaximo { get; set; }

        public int? Score { get; set; }

        public bool? forzamiento_solicitud { get; set; }

        public string USUARIO_MODIFICACION { get; set; }

        public int? VersionScore { get; set; }

        public string UrlFoto { get; set; }

        public string UrlFotoCedula { get; set; }

        public string UrlFotoSelfie { get; set; }

        public string UrlFotoFirma { get; set; }
        public string FotoFirma { get; set; }
        public int? TipoFoto { get; set; }

        public int? PIN { get; set; }

        public int? Telefono { get; set; }

        public float? UnMatchedFace { get; set; }
        public string ImageRotationSource { get; set; }
        public string ImageRotationTarget { get; set; }
        public string MatchedFace { get; set; }
        public float? PorcentMatched { get; set; }
        public float? PorcentUnmatched { get; set; }
        public float? PositionTop { get; set; }
        public float? PositionLeft { get; set; }
        public string Mensaje { get; set; }

        public bool result { get; set; }

        public string ResultTextCed { get; set; }

		public string DetectedText { get; set; }

		public bool? Paso_1 { get; set; }
		public bool? Paso_2 { get; set; }
		public bool? Paso_3 { get; set; }
		public bool? Paso_4 { get; set; }
		public bool? Paso_5 { get; set; }
		public bool? Paso_6 { get; set; }
		public bool? Paso_7 { get; set; }
		public bool? Paso_8 { get; set; }
		public bool? Paso_9 { get; set; }
		public bool? Paso_10 { get; set; }
		public bool? Paso_11 { get; set; }
		public bool? Paso_12 { get; set; }

		public string DeviceImageInfo { get; set; }
	}
}