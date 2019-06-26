using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models
{
    public class ParametrosRechazarSolicitud
    {
        public int Id { get; set; }
        public int status1 { get; set; }
        public int status2 { get; set; }
        public string UsuarioModifica { get; set; }
        public ParametrosRechazarSolicitud(int id,int status1, int status2, string user)
        {
            this.Id = id;
            this.status1 = status1;
            this.status2 = status2;
            this.UsuarioModifica = user;
        }
    }
}