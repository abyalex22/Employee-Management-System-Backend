namespace EmployeeManagement.API.ModelsV2.DTOs
{
    public class LoginResponse_v2
    {
        public int EmployeeId { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
