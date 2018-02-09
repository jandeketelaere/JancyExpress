using JancyExpress;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace JancyExpressSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.UseJancyExpress();

            RegisterRequestHandlers(services);
        }

        private static void RegisterRequestHandlers(IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<Handlers.HelloWorld.Get>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseJancyExpress(app =>
            {
                //app.Get("api/helloworld/{name}", serviceProvider.GetService<Handlers.HelloWorld.Get>());

                app.Get
                (
                    "api/helloworld/{name}",
                    serviceProvider.GetService<Handlers.HelloWorld.Get>(),
                    async (request, response, routeData, next) =>
                    {
                        //response.ContentType = "application/json";
                        await response.WriteAsync(JsonConvert.SerializeObject(new { name = "decorator2" }));
                        await next();
                    },
                    async (request, response, routeData, next) =>
                    {
                        response.ContentType = "application/json";
                        await response.WriteAsync(JsonConvert.SerializeObject(new { name = "decorator1" }));
                        await next();
                    }
                );
            });
        }
    }
}