using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using quizapp.Controllers;
using System;

namespace quizapp
{
    public class StartUp
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init()
        {
            var host = new HostBuilder().ConfigureServices((c, x) => { ConfigureServices(c, x); }).Build();
            ServiceProvider = host.Services;
        }

        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddSingleton<ICategoryController, CategoryController>();
            services.AddSingleton<IDifficultyController, DifficultyController>();
            services.AddSingleton<IQuestionController, QuestionController>();
            services.AddSingleton<IQuestionTypeController, QuestionTypeController>();
            services.AddHttpClient();
            services.BuildServiceProvider();
        }
    }
}
