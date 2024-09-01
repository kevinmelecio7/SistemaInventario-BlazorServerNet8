using AppLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLogin.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> Users { get; set; }
    }
}
