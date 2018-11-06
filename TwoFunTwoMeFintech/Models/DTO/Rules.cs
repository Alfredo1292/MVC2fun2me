using System;
using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Rules
    {
        [Key]
        public long IdRule { get; set; }

        public long? Modelo { get; set; }

        public string NombreModelo { get; set; }

        public string Descripcion { get; set; }

        public string Proceso { get; set; }

        public string Condicion { get; set; }

        public string Tipo { get; set; }

        public bool? Activo { get; set; }

		public string ActivoString { get; set; }
		public bool? Estado { get; set; }

		public int? OrdenEjecucion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

		public string Mensaje { get; set; }

		public string ACCION { get; set; }
	}
}