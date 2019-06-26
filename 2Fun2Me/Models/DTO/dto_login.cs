using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMe.Models.DTO
{
    public class dto_login
    {
        public string cod_agente { get; set; }

        public string nombre { get; set; }

        public string pass { get; set; }

        public string vendedor { get; set; }

        public DateTime? fecha_cambiopassword { get; set; }

        public string STR_USUARIO_AD { get; set; }

        public string STR_COD_AGENTE_PADRE { get; set; }

        public bool? BIT_DISPONIBLE { get; set; }

        public int? INT_COD_AREA { get; set; }

        public DateTime? FEC_CREACION { get; set; }

        public DateTime? FEC_MODIFICACION { get; set; }

        public string estado { get; set; }

        public string correo { get; set; }
    }
}