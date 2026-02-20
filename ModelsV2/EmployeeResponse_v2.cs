namespace EmployeeManagement.API.ModelsV2.DTOs
{
    public class EmployeeResponse_v2
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string Status { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Role { get; set; } = null!;

        public string? SkillSet { get; set; }

    }
}
