//namespace EmployeeManagement.API.Models
//{
//    public class LoginRequest
//    {
//        public string Username { get; set; } = null!;
//        public string Password { get; set; } = null!;
//    }
//}

using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; } = null!;
    }
}
