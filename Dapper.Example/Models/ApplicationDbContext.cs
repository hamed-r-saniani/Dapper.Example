using Microsoft.EntityFrameworkCore;

namespace Dapper.Example.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Ignore(x=>x.Employees);
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CompanyId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
