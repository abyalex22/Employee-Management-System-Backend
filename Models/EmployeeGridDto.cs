namespace EmployeeManagement.API.Models
{
    public class EmployeeGridDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
