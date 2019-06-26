using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionPersona
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

        public int IdSolicitud { get; set; }
        public int intIdProducto { get; set; }
        public string UsoCredito { get; set; }
        public string OrdenPatronal { get; set; }
        public int IdTipRefFamiliar { get; set; }
        public string NombreCompleto { get; set; }
        public string TelefonoFamiliar { get; set; }
        public string Empresa { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string SupervisorDirecto { get; set; }
        public string NombreCompletoPersonal { get; set; }
        public string TelefonoPersonal { get; set; }
    }
}