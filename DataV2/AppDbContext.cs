using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.ModelsV2;

namespace EmployeeManagement.API.DataV2
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeEntity> Employees { get; set; }
        

    }
}
