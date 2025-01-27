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

            // Регистрируем нужные нам маршруты
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
