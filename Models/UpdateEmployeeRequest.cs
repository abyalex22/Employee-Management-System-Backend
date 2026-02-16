namespace EmployeeManagement.API.Models
{
    public class UpdateEmployeeRequest
    {
        public string Name { get; set; } = null!;
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? SkillSet { get; set; }

        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string ModifiedBy { get; set; } = null!;
    }
}
