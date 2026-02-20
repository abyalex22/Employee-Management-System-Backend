using EmployeeManagement.API.DataV2;
using EmployeeManagement.API.ModelsV2;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.ServicesV2
{
    public class AuthService_v2
    {
        private readonly AppDbContext _context;

        public AuthService_v2(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeEntity?> GetUserByUsername(string username)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(x => x.Username == username);
        }
    }
}
