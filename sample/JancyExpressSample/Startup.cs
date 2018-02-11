using JancyExpress;
using JancyExpressSample.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;

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

            RegisterLogging(services);
            RegisterRequestHandlers(services);
            RegisterMiddleware(services);
        }

        private static void RegisterLogging(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            services.AddSingleton<IJancyLogger, JancyLogger>();
        }

        private static void RegisterRequestHandlers(IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<Handlers.HelloWorld.Get>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        private static void RegisterMiddleware(IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<Middleware.ExceptionMiddleware>()
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

                app.Get("api/helloworld2/{name}",
                    serviceProvider.GetService<Handlers.HelloWorld.Get>().Handle());

                app.Get("api/helloworld/{name}",
                    async (request, response, routeData, next) =>
                    {
                        await next();
                    },
                    async (request, response, routeData, next) =>
                    {
                        await next();
                    },
                    serviceProvider.GetService<Handlers.HelloWorld.Get>().Handle()
                );
            });
        }
    }
}