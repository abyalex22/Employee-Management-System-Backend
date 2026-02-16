using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models
{
    public class RegisterEmployeeRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Designation is required")]
        [StringLength(50)]
        public string Designation { get; set; } = null!;

        //public string? Address { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string Address { get; set; } = null!;


        [Required(ErrorMessage = "Department is required")]
        [StringLength(50)]
        public string Department { get; set; } = null!;

        [Required(ErrorMessage = "Joining Date is required")]
        public DateTime? JoiningDate { get; set; }

        [Required(ErrorMessage = "Skill Set is required")]
        public string SkillSet { get; set; } = null!;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; } = null!;

        public string? ProfilePhoto { get; set; }
    }
}
