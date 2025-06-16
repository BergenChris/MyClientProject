using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using MyClientProject.Services.Interfaces;



namespace MyClientProject.Services
{
    public class SeedService : ISeedService
    {
        private readonly ShopDbContext dbContext;

        public SeedService(ShopDbContext context)
        {
            dbContext = context;
        }

        public async Task SeedDatabaseAsync()
        {
            
                dbContext.Database.ExecuteSqlRaw("DELETE FROM [Users]");
                dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Users', RESEED, 0)");
                dbContext.Database.ExecuteSqlRaw("DELETE FROM [ShippingAdresses]");
                dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('ShippingAdresses', RESEED, 0)");
                dbContext.Database.ExecuteSqlRaw("DELETE FROM [Items]");
                dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('items', RESEED, 0)");
                dbContext.Database.ExecuteSqlRaw("DELETE FROM [Orders]");
                dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Orders', RESEED, 0)");
                dbContext.Database.ExecuteSqlRaw("DELETE FROM [Stores]");
                dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Stores', RESEED, 0)");


                var seeder = new DataSeeder(dbContext);
                seeder.SeedItemsFromJson();
                await dbContext.SaveChangesAsync();
            
        }
    }
}
