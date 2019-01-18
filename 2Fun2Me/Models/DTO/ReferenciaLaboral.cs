using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMe.Models.DTO
{
	public class ReferenciaLaboral
	{
		[Required]
		public string IdTipoIdentificacion { set; get; }
		[Required]
		public string Identificacion { set; get; }
		[Required]
		public string Empresa { get; set; }
		[Required]
		public int? Telefono { get; set; }

		public string SupervisorDirecto { get; set; }

		public DateTime? FechaIngreso { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public string UsrModifica { get; set; }

		public string Mensaje { get; set; }

	}
}