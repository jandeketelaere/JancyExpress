using JancyExpress;
using JancyExpress.Configuration;
using JancyExpress.Extensions;
using JancyExpressSample.Infrastructure;
using JancyExpressSample.Middleware.ApiHandler;
using JancyExpressSample.Middleware.HttpHandler;
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
            RegisterHandlers(services, assembly);
            RegisterMiddleware(services, assembly);
            RegisterConfiguration(services);
        }

        private static void RegisterConfiguration(IServiceCollection services)
        {
            services.AddScoped<ServiceFactory>(p => p.GetService);

            var configuration = new JancyExpressConfiguration(config =>
            {
                config.App(app =>
                {
                    app.Use()
                        .WithHttpHandlerMiddleware<ExceptionMiddleware>()
                        .WithHttpHandlerMiddleware<RequestResponseLoggingMiddleware>()
                        .WithApiHandlerMiddleware(typeof(TransactionMiddleware<,>));

                    app.Get("api/apple/simpleget/{name}")
                        .WithHttpHandlerMiddleware<Features.Apple.SimpleGet.HttpSecurity>()
                        .WithHttpHandler<Features.Apple.SimpleGet.HttpHandler>()
                        .WithApiHandlerMiddleware<Features.Apple.SimpleGet.Validator>()
                        .WithApiHandler<Features.Apple.SimpleGet.ApiHandler>();

                    app.Post("api/apple/simplepost")
                        .WithHttpHandler<Features.Apple.SimplePost.HttpHandler>()
                        .WithApiHandlerMiddleware<Features.Apple.SimplePost.Validator>()
                        .WithApiHandler<Features.Apple.SimplePost.ApiHandler>();
                });
            });

            services.AddSingleton(configuration);
        }

        private static void RegisterLogging(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
            services.AddSingleton<IJancyLogger, JancyLogger>();
        }

        private static void RegisterMiddleware(IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IHttpHandlerMiddleware)))
                .AsSelf()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IApiHandlerMiddleware<,>)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IHttpHandler<,>)))
                .AsSelf()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IApiHandler<,>)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseJancyExpress(serviceProvider);
        }
    }
}