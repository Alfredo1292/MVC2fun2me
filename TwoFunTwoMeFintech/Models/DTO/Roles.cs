using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Roles
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Nombre Rol"), Required]
        public string ROLESNOMBRE { get; set; }
        public string Mensaje { get; set; }
        public string ACCION { get; set; }
        public string ROLES { get; set; }
        
    }
    public class UserRoles
    {
        public List<Roles> rolesUsuario { get; set; }
    }
}