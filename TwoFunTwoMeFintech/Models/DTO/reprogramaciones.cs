using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class reprogramaciones
    {

        public string Identificacion { set; get; }
        public int IdCredito { set; get; }
        public int Cantidad { set; get; }
        public string AgenteAsignado { set; get; }
        public string Fecha { set; get; }
    }
}