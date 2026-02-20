using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.ModelsV2.Requests
{
    public class CreateEmployeeRequest_v2
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Designation { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? SkillSet { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
