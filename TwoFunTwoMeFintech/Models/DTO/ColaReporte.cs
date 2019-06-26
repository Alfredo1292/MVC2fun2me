using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class ColaReporte
    {
        public int IdReporte { get; set; }      
        public string NombreArchivo { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
    }
}