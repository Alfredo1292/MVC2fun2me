using System;
using System.Collections.Generic;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Tipos
    {
        public int Id { get; set; }

        //public int Id_Estado { get; set; }

        public int? ID_ESTADO { get; set; }

        public string Descripcion { get; set; }

        public string Otros { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaModificacion { get; set; }
        public string MensajeError { get; set; }
		public string Mensaje { get; set; }

		public List<Tipos> ListTipos { get; set; }

		
	}
	public class TipoMoneda
	{
		public int Id { get; set; }

		public string Descripcion { get; set; }

		public string Respuesta { get; set; }
	}
	public class TipoCuentas
	{
		public int Id { get; set; }

		public string Descripcion { get; set; }

		public string Respuesta { get; set; }
	}
	public class Tab_StatusDispAgente
	{
		public int IdDispAgente { get; set; }

		public string cod_agente { get; set; }

		public string IdCatDisponibilidad { get; set; }

		public DateTime FechaIngreso { get; set; }

		public DateTime? FechaModificacion { get; set; }

		public List<Tab_CatDisponibilidad> ListCatDisponibilidad { get; set; }

		public bool Activo { get; set; }

	}
	public class Tab_CatDisponibilidad
	{
		public int IdCatDisponibilidad { get; set; }

		public string Descripcion { get; set; }

		public string Valida { get; set; }

		public bool Activo { get; set; }

	}

	//Avargas inicio clases para cargar los combos de los parametros del producto
	public class MontoCredito
	{
		public int IdMontoCredito { get; set; }

		public string Descripcion { get; set; }

		public decimal Monto { get; set; }

		public bool? Activo { get; set; }

		public decimal? MontoMaximo { get; set; }

		public List<MontoCredito> ListMontoCredito { get; set; }

		public List<PlazoCredito> ListPlazoCredito { get; set; }

		public List<FrecuenciaCredito> ListFrecuenciaCredito { get; set; }

		public string Respuesta { set; get; }
	}

	public class PlazoCredito
	{
		public int IdPlazoCredito { get; set; }

		public int IdMontoCredito { get; set; }

		public string Descripcion { get; set; }

		public bool? Activo { get; set; }

		public string Respuesta { set; get; }

        public decimal MontoCredito { get; set; }

        public int PlazoDias { get; set; }
    }

	public class FrecuenciaCredito
	{
		public int IdFrecuencia { get; set; }

		public string Descripcion { get; set; }

		public bool? Activo { get; set; }

	}
	//Avargas Fin clases para cargar los combos de los parametros del producto
	public class CatalogoTelefono
	{
		public int Id { get; set; }

		public string Descripcion { get; set; }

		public bool? Activo { get; set; }
	}
	public class CatalogoDireccion
	{
		public int Id { get; set; }
		public string Descripcion { get; set; }
		public bool? Activo { get; set; }
	}
}