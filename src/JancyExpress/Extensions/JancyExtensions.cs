using JancyExpress.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JancyExpress.Extensions
{
    public static class JancyExtensions
    {
        public static IServiceCollection UseJancyExpress(this IServiceCollection services)
        {
            return services.AddRouting();
        }

        public static IApplicationBuilder UseJancyExpress(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            var routeBuilder = new RouteBuilder(applicationBuilder);
                        
            var serviceFactory = serviceProvider.GetService<ServiceFactory>();
            var configuration = serviceFactory.GetInstance<JancyExpressConfiguration>();

            var routesGenerator = new JancyExpressRoutesGenerator(configuration, serviceFactory);
            
            foreach (var route in routesGenerator.GenerateRoutes())
            {
                routeBuilder.MapVerb(route.Verb, route.Template, route.Handler);
            }

            applicationBuilder.UseRouter(routeBuilder.Build());

            return applicationBuilder;
        }
    }
}