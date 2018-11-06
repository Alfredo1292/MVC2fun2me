using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
	public class clsSUBMENU
	{
		[Key]
		public int? ID { get; set; }

		public string SUBMENU { get; set; }

		public string CONTROLLER { get; set; }

		public int? MAINMENUID { get; set; }

		[Display(Name = "Menú Principal"), Required]
		public string descMAINMENUID { get; set; }

		public int? ROLEID { get; set; }

		[Display(Name = "Rol"), Required]
		public string descROLEID { get; set; }

		[Display(Name = "Acción"), Required]
		public string ACTION { get; set; }
		public string ACCION { get; set; }
		public string Mensaje { get; set; }

		public List<clsSUBMENU> listadoSubMenu { get; set; }
		public List<clsMAINMENU> listadoMainMenu { get; set; }

		public List<Roles> ListRoles { get; set; }

	}
}