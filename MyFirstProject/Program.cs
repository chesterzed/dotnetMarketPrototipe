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

            // Подключаем в нашу конфиг. файл appsetting.json
            IConfigurationBuilder configbuild = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = configbuild.Build();
            AppConfig config = configuration.GetSection("Project").Get<AppConfig>()!;
            // ' ! '  Даёт указание компилятору,
            // что мы точно знаем что наш конфиг подгрузился configuration не пустой

            // Подклюачем контекст к бд
            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(config.Database.ConnectionString)
                // На момент создания приложения в данной версии EF был баг, хотя ошибки нет, поэтому подавляем предупреждения
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

            // 
            builder.Services.AddTransient<IServiceCategoriesRepository, EFserviceCategoriesRepository>();
            builder.Services.AddTransient<IServicesRepository, EFservicesRepository>();
            builder.Services.AddTransient<DataManager>();


            // Настраиваем Identity Систему
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // Настраиваем Auth cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/admin/accessdenied";
                options.SlidingExpiration = true;
            });

            // Подключаем функционал контроллеров
            builder.Services.AddControllersWithViews();

            // Собираем конфигурацию
            WebApplication app = builder.Build();

            // ! Порядок следования middleware очень важен,
            // они будут выполняться согласно нему

            // Подключаем использование статичных файлов (js, css, b nl)
            app.UseStaticFiles();

            // Для того чтобы приложение определяло правильно контроллер
            // Подключаем систему маршрутизации
            app.UseRouting();

            //Подключаем систему аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // Регистрируем нужные нам маршруты
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
