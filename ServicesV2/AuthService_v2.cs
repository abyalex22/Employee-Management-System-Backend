using EmployeeManagement.API.DataV2;
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

        public async Task<(int id, string role, string status, string hash)?>
            ValidateLogin(string username)
        {
            var user = await _context.Employees
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            return (user.EmployeeId, user.Role, user.Status, user.PasswordHash);
        }
    }
}
