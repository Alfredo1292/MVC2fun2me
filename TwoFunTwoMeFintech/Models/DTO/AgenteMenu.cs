using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
	public class AgenteMenu
	{
		[Key]
		public int? ID { get; set; }

		public string COD_AGENTE { get; set; }
		
        public string SUBMENU { get; set; }

		public int? IDSUBMENU { get; set; }

		public string Mensaje { get; set; }

		public string ACCION { get; set; }

        public int? IdRole { get; set; }
		public List<AgenteMenu> ListAgenteMenuAsig { get; set; }

		public List<AgenteMenu> ListAgenteMenuDesAsig { get; set; }

		public List<dto_login> Listdto_Login { get; set; }
	}
}