using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMe.Models.DTO
{
	public class CuentaBancaria
	{
      [Key]
		public int? Id { get; set; }

		public int? IdPersona { get; set; }
		[Required]
		public string IdTipoIdentificacion { set; get; }
		[Required]
		public string Identificacion { set; get; }

		public int? IdBanco { get; set; }

		public string Cuenta { get; set; }

		public string CuentaSinpe { get; set; }

		public string CuentaIban { get; set; }

		public DateTime? FechaIngreso { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public string UsrModifica { get; set; }
		public char? Operaciones { get; set; }

		public bool? Predeterminado { get; set; }

		public int? TipoMoneda { get; set; }

		public int? TipoCuenta { get; set; }

		public string Mensaje { get; set; }

		public bool result { get; set; }

	}
}