﻿using JancyExpress;
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
            RegisterRouters(services, assembly);
        }

        private static void RegisterConfiguration(IServiceCollection services)
        {
            var configuration = new JancyExpressConfiguration(config =>
            {
                config.ValidateOnStartup = false;
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

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IHttpHandler)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        private static void RegisterRouters(IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<JancyExpressRouter>())
                .As<JancyExpressRouter>()
                .WithSingletonLifetime()
            );

            services.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<JancyExpressGlobalRouter>())
                .As<JancyExpressGlobalRouter>()
                .WithSingletonLifetime()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseJancyExpress();
        }
    }
}