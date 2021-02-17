using Microsoft.EntityFrameworkCore;
using angular_netcore.Models;

namespace angular_netcore.Repository
{
    public class AngularNetCoreDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        public AngularNetCoreDbContext (DbContextOptions<AngularNetCoreDbContext> options) : base(options) { }
    }
}