namespace EmployeeManagement.API.Models
{
    public class LoginResponse
    {
        public int EmployeeId { get; set; }
        public string Role { get; set; } = null!;
    }
}
