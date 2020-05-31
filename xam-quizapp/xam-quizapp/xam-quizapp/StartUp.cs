using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using quizapp.Controllers;
using quizapp.DbControllers;
using quizapp.DependancyService;
using System;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace quizapp
{
    public class StartUp
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init()
        {
            var a = Assembly.GetExecutingAssembly();
            var stream = a.GetManifestResourceStream("xam-quizapp.appsettings.json");

            var host = new HostBuilder()
            .ConfigureHostConfiguration(c =>
            {
                // Tell the host configuration where to file the file (this is required for Xamarin apps)
                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                //read in the configuration file!
                c.AddJsonStream(stream);
            })
            .ConfigureServices((c, x) =>
            {
                // Configure our local services and access the host configuration
                ConfigureServices(c, x);
            })
            .ConfigureLogging(l => l.AddConsole(o =>
            {
                //setup a console logger and disable colors since they don't have any colors in VS
                o.DisableColors = true;
            }))
            .Build();
            ServiceProvider = host.Services;
        }

        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            Xamarin.Forms.DependencyService.Get<IDataStorage>().CopyFilesToInstalled();
            services.AddSingleton<ICategoryController, CategoryController>();
            services.AddSingleton<IDifficultyController, DifficultyController>();
            services.AddSingleton<IQuestionController, QuestionController>();
            services.AddSingleton<IQuestionTypeController, QuestionTypeController>();
            services.AddSingleton<IPlayerStatsDbController, PlayerStatsDbController>();
            services.AddSingleton<ICategoryStatsDbController, CategoryStatsDbController>();
            services.AddHttpClient();
            services.BuildServiceProvider();
        }
    }
}
