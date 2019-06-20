using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class HistoricoSolicitudes
    {
        public string Identificacion { get; set; }
        public int IdSolicitud { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Descripcion { get; set; }
        public string FechaIngreso { get; set; }
        public string Origen { get; set; }
    }
}