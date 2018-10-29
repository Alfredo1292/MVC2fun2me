using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Roles
    {
        public int ID { get; set; }

        public string ROLES { get; set; }
    }
    public class UserRoles
    {
        public List<Roles> rolesUsuario { get; set; }
    }
}