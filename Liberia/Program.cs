using BLL.CustomService;
using BLL.ICustomService;
using DAL.Settings;
using Liberia.Data;
using Liberia.Helpers;
using Liberia.Seeds;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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

			//Configure Lockout
			builder.Services.Configure<IdentityOptions>(options =>
			{
				// Default Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;
			});

			//Cookie Configuration
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.AccessDeniedPath = "/Identity/Account/AccessDenied";
				options.Cookie.Name = "LiberiaCookies";
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.LoginPath = "/Identity/Account/Login";
				// ReturnUrlParameter requires 
				//using Microsoft.AspNetCore.Authentication.Cookies;
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
				options.SlidingExpiration = true;
			});

			builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
			opt.ValidationInterval = TimeSpan.Zero);

			builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

			builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

			//Custom Services
			builder.Services.AddTransient(typeof(IImageService), typeof(ImageService));
			builder.Services.AddTransient(typeof(IEmailSender), typeof(EmailSender));
			builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

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