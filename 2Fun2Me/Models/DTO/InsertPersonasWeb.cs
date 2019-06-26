using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMe.Models.DTO
{
    public class InsertPersonasWeb
    {
		[Required]
		public string IdTipoIdentificacion { set; get; }
		[Required]
		public string Identificacion { set; get; }
		[Required]
		public string PrimerNombre { set; get; }

		public string SegundoNombre { set; get; }
		[Required]
		public string PrimerApellido { set; get; }
		
		public string SegundoApellido { set; get; }
		[Required]
		public string TelefonoCel { set; get; }
		[Required]
		public string Correo { set; get; }
		[Required]
		public int? IdProducto { set; get; }

		public string IdBanco { set; get; }

		public string UsrModifica { set; get; }

		public string Origen { set; get; }

		public string Status { set; get; }
	}
}