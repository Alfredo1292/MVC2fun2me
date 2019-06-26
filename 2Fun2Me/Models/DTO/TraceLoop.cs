using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMe.Models.DTO
{
	public class TraceLoop
	{
		public string Paso { get; set; }

		public string Evento { get; set; }

		public string Data { get; set; }

		public string identificacion { get; set; }

		public string Solicitud { get; set; }

		public string traceId { get; set; }

		public string Mensaje { get; set; }

		public string Descripcion { get; set; }

		public string Estado { set; get; }

	}

}