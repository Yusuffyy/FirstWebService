using FirstWebServiceData.Models.Entity;
using Microsoft.EntityFrameworkCore;


namespace FirstWebServiceData.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
