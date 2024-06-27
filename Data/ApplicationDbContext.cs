using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ProductManagement.db");
            base.OnConfiguring(optionsBuilder);

        }
    }
}
