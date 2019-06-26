using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class DashboardVentas
    {
        public string cod_agente { get; set; }
        public int CantidadVentas { get; set; }
        public string Frecuencia { get; set; }
        public string Porcentaje { get; set; }
        public string Comportamiento { get; set; }
        public int MetaMensual { get; set; }
        public int MetaDiaria { get; set; }
    }
}