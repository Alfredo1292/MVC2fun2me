using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class GestionesAprobadas
	{
		[Key]
		public int? IdGestionesAprobadas { get; set; }
		
         public int? IdGestionGeneral { get; set; }

		public int? IdSolicitud { get; set; }

		public string Cod_Agente { get; set; }

		public string Mensaje { get; set; }

		public string nombre { get; set; }

		public string Detalle { get; set; }
		public DateTime? FechaAprobacion { get; set; }

		public bool? Activo { get; set; }
		
	    public string ACCION { get; set; }


		public List<GestionesAprobadas> ListGestionesAprobadas { get; set; }
	}
}