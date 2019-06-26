using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class ConfiguracionReportes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Proceso { get; set; }
        public string NombreArchivo { get; set; }
        public string Area { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
    }
}