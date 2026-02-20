using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.ModelsV2.DTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Designation { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? SkillSet { get; set; }
        public string CreatedBy { get; set; } = null!;


        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
