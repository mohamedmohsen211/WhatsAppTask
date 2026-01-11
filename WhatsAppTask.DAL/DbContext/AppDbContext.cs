using Microsoft.EntityFrameworkCore;
using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.DAL.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
