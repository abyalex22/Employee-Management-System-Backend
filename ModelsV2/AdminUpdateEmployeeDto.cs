namespace EmployeeManagement.API.ModelsV2.DTOs
{
    public class AdminUpdateEmployeeDto
    {
        public string Name { get; set; } = "";
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? SkillSet { get; set; }

        public string Role { get; set; } = "Employee";  
        public string Status { get; set; } = "Active";   
    }
}
