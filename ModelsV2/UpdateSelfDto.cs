namespace EmployeeManagement.API.ModelsV2
{
    public class UpdateSelfDto
    {
        public string Name { get; set; } = "";
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? SkillSet { get; set; }
    }
}
