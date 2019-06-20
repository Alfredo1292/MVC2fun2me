using System;
using System.Collections.Generic;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Bancos
	{
		public int Id { get; set; }

		public string Descripcion { get; set; }

		public int? IdPais { get; set; }

		public DateTime? FechaIngreso { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public string UsrModifica { get; set; }

		public string CodBanco { get; set; }

		public bool? BanAcep { get; set; }

		public string Respuesta { get; set; }

		public List<Bancos> ListBancos { get; set; }

		public List<TipoMoneda> ListTipoMoneda { get; set; }

		public List<TipoCuentas> ListTipoCuentas { get; set; }
	}
}