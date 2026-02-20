using EmployeeManagement.API.ModelsV2;
using EmployeeManagement.API.ModelsV2.DTOs;
using EmployeeManagement.API.ServicesV2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v2/employees")]
    public class EmployeesController_v2 : ControllerBase
    {
        private readonly EmployeeService_v2 _service;

        public EmployeesController_v2(EmployeeService_v2 service)
        {
            _service = service;
        }

        /* GET ALL — ADMIN ONLY */

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll(
    int page = 1,
    int pageSize = 5,
    string? search = null)
        {
            var result = await _service.GetAll(page, pageSize, search);
            return Ok(result);
        }


        /* GET BY ID — ANY LOGGED USER */
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _service.GetById(id);
            if (emp == null) return NotFound();

            return Ok(emp);
        }

        /* CREATE */
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            var entity = new EmployeeEntity
            {
                Name = dto.Name,
                Designation = dto.Designation,
                Address = dto.Address,
                Department = dto.Department,
                JoiningDate = dto.JoiningDate,
                SkillSet = dto.SkillSet,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system",
                Role = "Employee",
                Status = "Active"
            };

            var created = await _service.Create(entity);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] AdminUpdateEmployeeDto dto)
        public async Task<IActionResult> Update(
    int id,
    [FromBody] EmployeeManagement.API.ModelsV2.AdminUpdateEmployeeDto dto)

        {
            var emp = await _service.GetById(id);
            if (emp == null)
                return NotFound();

            emp.Name = dto.Name;
            emp.Designation = dto.Designation;
            emp.Address = dto.Address;
            emp.Department = dto.Department;
            emp.JoiningDate = dto.JoiningDate;
            emp.SkillSet = dto.SkillSet;
            //emp.Role = dto.Role;
            //emp.Status = dto.Status;
            emp.Role = string.IsNullOrWhiteSpace(dto.Role) ? emp.Role : dto.Role;
            emp.Status = string.IsNullOrWhiteSpace(dto.Status) ? emp.Status : dto.Status;


            await _service.SaveAsync();  

            return Ok();
        }


        //UpdateEmployee by employee
        [Authorize]
        [HttpPut("self")]
        public async Task<IActionResult> UpdateSelf([FromBody] UpdateSelfDto dto)
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (idClaim == null)
                return Unauthorized();

            int id = int.Parse(idClaim.Value);

            var emp = await _service.GetById(id);
            if (emp == null)
                return NotFound();

            emp.Name = dto.Name;
            emp.Designation = dto.Designation;
            emp.Department = dto.Department;
            emp.Address = dto.Address;
            emp.SkillSet = dto.SkillSet;
            emp.JoiningDate = dto.JoiningDate;

            await _service.Update(id, emp);

            return Ok();
        }

        //photo Upload
        [Authorize]
        [HttpPut("{id}/photo")]
        public async Task<IActionResult> UpdatePhoto(int id, [FromBody] PhotoDto dto)
        {
            var emp = await _service.GetById(id);
            if (emp == null)
                return NotFound();

            if (string.IsNullOrEmpty(dto.Photo))
                return BadRequest("Photo required");

            emp.ProfilePhoto = Convert.FromBase64String(dto.Photo);

            await _service.SaveAsync();

            return Ok();
        }




        /* SOFT DELETE */
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var ok = await _service.SoftDelete(id);
            if (!ok) return NotFound();

            return Ok();
        }
    }
}
