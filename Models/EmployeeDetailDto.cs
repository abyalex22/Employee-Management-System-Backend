namespace EmployeeManagement.API.Models
{
    public class EmployeeDetailDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? SkillSet { get; set; }

        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public byte[]? ProfilePhoto { get; set; }
    }
}
