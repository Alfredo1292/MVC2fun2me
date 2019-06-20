namespace TwoFunTwoMeFintech.Models.DTO
{
    public class PlanPlagos
    {
        public int IdSolicitud { get; set; }
        public string FechaPago { get; set; }
		public string IdCredito { get; set; }

		public string FechaVencimiento { get; set; }

		public string MontoCapital { get; set; }

		public string MontoInteres { get; set; }

		public string MontoOriginacion { get; set; }

		public string MontoIntMora { get; set; }

		public string MontoCobrado { get; set; }
		  public string MontoCuota { get; set; }

		public string Cuota { get; set; }

		public string Usuario { get; set; }

		public decimal? CapitalOriginal { get; set; }

		public decimal? InteresOriginal { get; set; }

		public decimal? OriginacionOriginal { get; set; }

		public decimal? InteresMoraOriginal { get; set; }

		public decimal? CapitalCobrado { get; set; }

		public decimal? InteresCobrado { get; set; }

		public decimal? OriginacionCobrado { get; set; }

		public decimal? InteresMoraCobrado { get; set; }

		public decimal? AdelantoCapital { get; set; }

		public decimal? MontoCargoMora { get; set; }

		public decimal? CargoMoraOriginal { get; set; }

		public decimal? CargoMoraCobrado { get; set; }

	}
}