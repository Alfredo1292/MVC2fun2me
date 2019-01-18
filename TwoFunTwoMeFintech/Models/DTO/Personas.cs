using System;
using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Personas
    {
        [Key]
        public int Id { get; set; }

        public int ID_SOLICITUD { get; set; }

        public string NOMBRE { get; set; }

        public string Descripcion { get; set; }

        public string XML_TUCA { get; set; }

        public string FECHA_INGRESO_TUCA { get; set; }

        public string ESTADO_TUCA { get; set; }

        public string XML_CREDID { get; set; }

        public string FECHA_INGRESO_CREDID { get; set; }

        public string ESTADO_CREDID { get; set; }

        public int? IdTipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public DateTime? VencimientoIdentificacion { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public int? TelefonoCel { get; set; }

        public int? TelefonoFijo { get; set; }

        public int? TelefonoLaboral { get; set; }

        public string Correo { get; set; }

        public string CorreoOpcional { get; set; }

        public int? EstadoCivil { get; set; }

        public int? Sexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public int? Provincia { get; set; }

        public int? Canton { get; set; }

        public int? Distrito { get; set; }

        public string DetalleDireccion { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsrModifica { get; set; }

        public int? IdStatusOrden { get; set; }
        public string CapitalPendiente { get; set; }
    }
}