using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.AnalisisRules
{
    public class ConsultaRules
    {
        public string Identificacion { get; set; }
        public int IdSolicitud { get; set; }
        public string MotivoRechazo { get; set; }
        public string DescripcionMotivoRechazo { get; set; }
        public string FechaIngreso { get; set; }
        public string Descripcion { get; set; }
    }
}