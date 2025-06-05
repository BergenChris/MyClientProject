using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;
using MyClientProject.Models;
using MyClientProject.Repos;
using MyClientProject.Repos.Interfaces;
using System.Text.Json;
using System;
using MyClientProject.Services;

namespace MyClientProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ShopDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped<UserRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<ItemRepo>();
            builder.Services.AddScoped<IItemRepo, ItemRepo>();
            builder.Services.AddScoped<OrderRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddSession();
            builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ShopDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ShopDbContext>();
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
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            

            app.Run();
        }
    }
}
