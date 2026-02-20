namespace EmployeeManagement.API.ModelsV2.DTOs
{
    public class UpdateEmployeeDto
    {
        public string Name { get; set; } = null!;
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? SkillSet { get; set; }
    }
}
