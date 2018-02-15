using JancyExpress;
using JancyExpress.Extensions;
using JancyExpressSample.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;

namespace JancyExpressSample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.UseJancyExpress();

            var assembly = Assembly.GetAssembly(typeof(Startup));

            RegisterLogging(services);
            RegisterRequestHandlers(services, assembly);
            RegisterMiddleware(services, assembly);
        }

        private static void RegisterLogging(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            services.AddSingleton<IJancyLogger, JancyLogger>();
        }

        private static void RegisterRequestHandlers(IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        private static void RegisterMiddleware(IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandlerMiddleware)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseJancyExpress(app =>
            {
                app.Use(
                    serviceProvider.GetService<Middleware.ExceptionMiddleware>().Handle(),
                    serviceProvider.GetService<Middleware.RequestResponseLoggingMiddleware>().Handle());

                app.Get("api/apple/simpleget/{name}",
                    serviceProvider.GetService<Features.Apple.SimpleGet.Validator>().Handle(),
                    serviceProvider.GetService<Features.Apple.SimpleGet.Handler>().Handle()
                );
            });
        }
    }
}