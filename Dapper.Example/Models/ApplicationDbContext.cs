using Microsoft.EntityFrameworkCore;

namespace Dapper.Example.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
    }
}
