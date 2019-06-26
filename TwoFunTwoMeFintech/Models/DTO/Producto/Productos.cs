using System;

namespace TwoFunTwoMeFintech.Models.DTO.ProductosMantenimiento
{
    public class ProductosMantenimiento
	{
		public int Id { get; set; }

		public string NombreProducto { get; set; }

		public decimal? MontoProducto { get; set; }

		public int? PlazoDias { get; set; }

		public string Frecuencia { get; set; }

		public string FrecuenciaDias { get; set; }

		public int? CantidadCuotas { get; set; }

		public decimal? Originacion { get; set; }

		public decimal? Tasa { get; set; }

		public decimal? Tasadiaria { get; set; }

		public decimal? Cuota { get; set; }

		public int? IdMoneda { get; set; }

		public DateTime? FechaIngreso { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public string UsrModifica { get; set; }

		public decimal? Originacion2 { get; set; }

		public bool? Activo { get; set; }

		public string Respuesta { get; set; }

	}
}