using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
	public class clsMAINMENU
	{
		[Key]
		public int ID { get; set; }

		public string MAINMENU { get; set; }

		public string ACCION { get; set; }

		public string Mensaje { get; set; }
	}
}