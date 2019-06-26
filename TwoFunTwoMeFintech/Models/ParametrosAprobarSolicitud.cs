using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models
{
    public class ParametrosAprobarSolicitud
    {
        public int Id { get; set; }
        public string UsrModifica { get; set; }
        public string Comentario { get; set; }
        public int status1 { get; set; }
        public int status2 { get; set; }
        public string EstadoGestion { get; set; }
        public ParametrosAprobarSolicitud(int id, string user, int status1, int status2, string coment, string estadoGestion)
        {
            this.Id = id;
            this.UsrModifica = user;
            this.status1 = status1;
            this.status2 = status2;
            this.Comentario = coment;
            this.EstadoGestion = estadoGestion;

        }
    }

}