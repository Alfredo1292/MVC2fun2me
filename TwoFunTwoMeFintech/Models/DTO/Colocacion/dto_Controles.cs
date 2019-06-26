namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class dto_Controles
    {
		//INICIO FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA
		public string Accion { get; set; }

		public string Tipo { get; set; }
		public string CodControl { get; set; }
		public string Mask { get; set; }
		public string Text { get; set; }
		public string PlaceHolder { get; set; }
		public string ExprecionRegular { get; set; }
		public string Color { get; set; }
		public string Style { get; set; }
		public string Imagen { get; set; }
		public string Editable { get; set; }
		public string Visible { get; set; }
		public string MaxLength { get; set; }
		public string Class { get; set; }
	}
}