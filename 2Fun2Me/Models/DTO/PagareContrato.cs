
using System;

namespace TwoFunTwoMe.Models.DTO
{
	public class PagareContrato
	{
		public int? Id { set; get; }
		public int? IdSolicitud { set; get; }
		public DateTime fechagenerapagare { set; get;}
		public string IdTipoIdentificacion { set; get; }
        public string Identificacion { set; get; }
        public string Nombre { set; get; }
        public string Frecuencia { set; get; }
        public string Frecuencia2 { set; get; }
        public string Frecuencia3 { set; get; }
        public int CantidadCuotas { set; get; }
        public decimal Interes { set; get; }
        public string InteresLetras { set; get; }
        public decimal MontoProducto { set; get; }
        public decimal MontoProductoPagare { set; get; }
        public string MontoProductoLetras { set; get; }
        public string MontoProductoLetrasPagare { set; get; }
        public string Moneda { set; get; }
        public string CtaCliente { set; get; }
        public int MesesPlazo { set; get; }
        public decimal Cuota { set; get; }
        public string FechaPrimerPagoLetras { set; get; }
        public string FechaUltimoPagoLetras { set; get; }
        public string FechaHoy { set; get; }
        public string CuotaenLetras { set; get; }
        public string CantidadCuotasLetras { set; get; }
        public string Contrato { set; get; }
        public string Pagare { set; get; }
        public string PagoProximasCuotas { set; get; }
        public int SubStatus { set; get; }
        public decimal MontoMaximo { set; get; }
        public int TelefonoCel { set; get; }
        public string Correo { set; get; }
        public decimal MontoTotal { set; get; }
		public string file { set; get; }
		public string Mensaje { set; get; }
		public string FotoFirma { set; get; }

	}
}