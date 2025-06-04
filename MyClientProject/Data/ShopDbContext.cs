using Microsoft.EntityFrameworkCore;
using MyClientProject.Models;

namespace MyClientProject.Data
{
    public class ShopDbContext : DbContext
    {
        

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employer> Employees { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Order> Orders { get; set; }


        public DbSet<ShippingAdress> ShippingAdresses { get; set; }

        public ShopDbContext() { }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Employer>().ToTable("Employers");

            modelBuilder.Entity<ShippingAdress>()
                .HasOne(sa => sa.User)
                .WithMany(u => u.ShippingAdresses)
                .HasForeignKey(sa => sa.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ShippingAdress>()
                .HasOne(sa => sa.Store)
                .WithMany(s => s.ShippingAdresses)
                .HasForeignKey(sa => sa.StoreId)
                .OnDelete(DeleteBehavior.SetNull);

            

            

            

            base.OnModelCreating(modelBuilder);
        }
    }
}
