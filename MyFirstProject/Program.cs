using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyFirstProject.Domain;
using MyFirstProject.Domain.Repositories.Abstract;
using MyFirstProject.Domain.Repositories.EntityFramework;
using MyFirstProject.Infrastracture;

namespace MyFirstProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // connect configuration to file appsetting.json
            IConfigurationBuilder configbuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configbuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;
            // ' ! '  means that value can be nullable
            // It's very important if configuration file is empty

            // connect db context to db
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
                // On the project creating time. In current version of EF was an compilation error, turn of notification
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            // 
            builder.Services.AddTransient<IServiceCategoriesRepository, EFserviceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFservicesRepository>();
            builder.Services.AddTransient<DataManager>();


            // Set an Identity system
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // Set an Auth cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            // Add controllers functional
            builder.Services.AddControllersWithViews();
            // builder.Services.AddRazorPages();


            // Compile the configuration
            WebApplication app = builder.Build();

            // ! The order of middleware is very important,
            // application will build according this order

            // Add static files usage (js, css, b nl)
            app.UseStaticFiles();

            // ��� ���� ����� ���������� ���������� ��������� ����������
            // Add routing system
            app.UseRouting();

            // Add Authentication and Authorization
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // Register self necessary routes
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
