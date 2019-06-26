using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class ReprogramacionVentas
    {
        public int IdSolicitud { set; get; }
        public string CodAgente { set; get; }
        public DateTime FechaReprogramacion { set; get; }
        public string ComentarioReprogramacion { set; get; }
        public string CodAgenteOriginal { set; get; }
    }
}