using Microsoft.EntityFrameworkCore;
using SignalRDemo.Models;

namespace SignalRDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
