using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.API.ModelsV2
{
    [Table("Employees")]
    public class EmployeeEntity
    {
        [Key]
        public int EmployeeId { get; set; }

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
        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = "Employee";
        public string Status { get; set; } = "Active";

        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[]? ProfilePhoto { get; set; }


    }
}
