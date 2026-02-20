using EmployeeManagement.API.DataV2;
using EmployeeManagement.API.ModelsV2;
using EmployeeManagement.API.ModelsV2.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.ServicesV2
{
    public class EmployeeService_v2
    {
        private readonly AppDbContext _context;

        public EmployeeService_v2(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult_v2<EmployeeResponse_v2>> GetAll(
            int page = 1,
            int pageSize = 5,
            string? search = null)
        {
            //var query = _context.Employees.AsQueryable();
            var query = _context.Employees
    .Where(x => x.Role != "Admin")
    .AsQueryable();


            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Name.Contains(search));

            var total = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.EmployeeId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                //.Select(x => new EmployeeResponse_v2
                //{
                //    EmployeeId = x.EmployeeId,
                //    Name = x.Name,
                //    Designation = x.Designation,
                //    Department = x.Department,
                //    Address = x.Address,
                //    SkillSet = x.SkillSet,
                //    JoiningDate = x.JoiningDate,
                //    Status = x.Status
                //})
                .Select(x => new EmployeeResponse_v2
                {
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    Designation = x.Designation,
                    Department = x.Department,
                    Address = x.Address,
                    SkillSet = x.SkillSet,
                    JoiningDate = x.JoiningDate,
                    Status = x.Status,
                    Role = x.Role
                })

                .ToListAsync();

            return new PagedResult_v2<EmployeeResponse_v2>
            {
                Data = data,
                TotalCount = total
            };
        }

        public async Task<EmployeeEntity?> GetById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<EmployeeEntity> Create(EmployeeEntity emp)
        {
            emp.CreatedAt = DateTime.UtcNow;

            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            return emp;
        }

        public async Task<bool> Update(int id, EmployeeEntity updated)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return false;

            emp.Name = updated.Name;
            emp.Designation = updated.Designation;
            emp.Department = updated.Department;
            emp.Address = updated.Address;
            emp.SkillSet = updated.SkillSet;
            emp.JoiningDate = updated.JoiningDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDelete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return false;

            emp.Status = "Inactive";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
