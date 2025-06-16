
using Microsoft.EntityFrameworkCore;
using MyClientProject.Data;

using MyClientProject.Repos;
using MyClientProject.Repos.Interfaces;

using MyClientProject.Services;
using MyClientProject.Services.Interfaces;

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
            builder.Services.AddScoped < ISeedService,SeedService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IItemRepo, ItemRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddSession();
            builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ShopDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


          

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
