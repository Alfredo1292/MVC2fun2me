using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class TelefonoLaboral
    {
        public string Identificacion { get; set; }
        public string Telefono { get; set; }
        public string ComentarioTelefonoLaboral { get; set; }
        public string UsuarioIngreso { get; set; }
        public string FechaIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }
        public string MensajeSalida { get; set; }
        public int? IdSolicitud { get; set; }
    }
}