using EmployeeManagement.API.ModelsV2;
using EmployeeManagement.API.ModelsV2.DTOs;

namespace EmployeeManagement.API.HelpersV2
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(EmployeeEntity e)
        {
            return new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                Designation = e.Designation,
                Department = e.Department,
                Address = e.Address,
                JoiningDate = e.JoiningDate,
                SkillSet = e.SkillSet,
                Role = e.Role,
                Status = e.Status
            };
        }

        public static EmployeeEntity ToEntity(CreateEmployeeDto dto)
        {
            return new EmployeeEntity
            {
                Name = dto.Name,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                Role = "Employee",
                Status = "Active"
            };
        }

        public static void UpdateEntity(EmployeeEntity entity, UpdateEmployeeDto dto)
        {
            entity.Name = dto.Name;
            entity.Designation = dto.Designation;
            entity.Department = dto.Department;
            entity.Address = dto.Address;
            entity.JoiningDate = dto.JoiningDate;
            entity.SkillSet = dto.SkillSet;
        }
    }
}
