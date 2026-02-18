namespace EmployeeManagement.API.Models
{
    public class LoginResponse
    {
        public int EmployeeId { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;   

        public string Token { get; set; } = null!;    // JWT
    }
}
