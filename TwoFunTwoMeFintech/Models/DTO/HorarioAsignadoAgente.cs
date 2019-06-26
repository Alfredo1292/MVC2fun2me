using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class HorarioAsignadoAgente
    {
        public int Id { get; set; }
        public string cod_agente { get; set; }
        public string lunes { get; set; }
        public string martes { get; set; }
        public string miercoles { get; set; }
        public string jueves { get; set; }
        public string viernes { get; set; }
        public string sabado { get; set; }
        public string domingo { get; set; }
    }
}