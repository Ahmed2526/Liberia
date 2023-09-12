using BLL.CustomService;
using BLL.ICustomService;
using Liberia.Consts;
using Liberia.Data;
using Liberia.Helpers;
using Liberia.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Liberia
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("NewConn") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI().AddDefaultTokenProviders();

            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            //Custom Services
            builder.Services.AddScoped(typeof(IImageService), typeof(ImageService));


            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Seed Roles And Admin Users.
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await DefaultRoles.SeedRoles(roleManager);
            await DefaultUsers.SeedAdminUser(UserManager);

            app.MapControllerRoute
                (
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.MapRazorPages();

            app.Run();
        }
    }
}