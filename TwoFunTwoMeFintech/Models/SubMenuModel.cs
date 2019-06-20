using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models
{
    public class SubMenuModel
    {
        public int ID { get; set; }

        public string SUBMENU { get; set; }

        public string CONTROLLER { get; set; }

        public int? MAINMENUID { get; set; }

        public int? ROLEID { get; set; }

        public string ACTION { get; set; }

        public string cod_agente { get; set; }

    }
}