using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class UserAgencias
    {
        public int? IdT_AgenciasExternas { get; set; }
        public string Nombre { get; set; }
        public string DescripAgencia { get; set; }
    }
    public class ListUserRoles
    {
        public List<Roles> rolesUsuario { get; set; }
    }
}