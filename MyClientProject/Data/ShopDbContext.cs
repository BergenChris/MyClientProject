using Microsoft.EntityFrameworkCore;
using MyClientProject.Models;

namespace MyClientProject.Data
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Item> Items { get; set; }

        public ShopDbContext() { }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }
    }
}
