using Microsoft.EntityFrameworkCore;
using PRUEBAGS.Models;

namespace PRUEBAGS.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
