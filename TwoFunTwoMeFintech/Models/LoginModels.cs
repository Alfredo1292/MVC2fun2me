using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models
{
    public class LoginModels
    {
        public int UserId { get; set; }
        [Required(ErrorMessage="Please enter the User Name")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "Please enter the Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int? UserRoleId { get; set; }
        public string RoleName { get; set; }
        public string RememberMe { get; set; }
    }
}