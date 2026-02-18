using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.DataV2;
using EmployeeManagement.API.ModelsV2;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/v2/employees")]
    [Authorize]
    public class EmployeesController_v2 : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController_v2(AppDbContext context)
        {
            _context = context;
        }

        /* GET ALL — ADMIN ONLY */
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Employees.ToListAsync();
            return Ok(list);
        }

        /* GET BY ID — ANY LOGGED USER */
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();

            return Ok(emp);
        }

        /* CREATE */
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeEntity employee)
        {
            employee.CreatedAt = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        /* UPDATE — ADMIN ONLY */
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] EmployeeEntity updated)
        {
            if (id != updated.EmployeeId)
                return BadRequest();

            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /* SOFT DELETE */
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete([FromRoute] int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            emp.Status = "Inactive";

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
