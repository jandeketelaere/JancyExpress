using JancyExpress.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress.Extensions
{
    public static class JancyExtensions
    {
        public static IServiceCollection UseJancyExpress(this IServiceCollection services)
        {
            return services.AddRouting();
        }

        public static IApplicationBuilder UseJancyExpress(this IApplicationBuilder applicationBuilder)
        {
            var routeBuilder = new RouteBuilder(applicationBuilder);
            
            var configuration = applicationBuilder.ApplicationServices.GetService<JancyExpressConfiguration>();

            if (configuration.ValidateOnStartup)
                configuration.Validate();

            var routesGenerator = new JancyExpressRoutesGenerator();

            var routers = applicationBuilder.ApplicationServices.GetServices<JancyExpressRouter>().Cast<IJancyExpressRouter>();
            var globalRouter = applicationBuilder.ApplicationServices.GetService<JancyExpressGlobalRouter>() as IJancyExpressGlobalRouter;
            
            foreach (var route in routesGenerator.GenerateRoutes(routers.Select(r => r.GetConfiguration()).ToList(), globalRouter?.GetConfiguration() ?? new JancyExpressGlobalRouterConfiguration(new JancyExpressGlobalRoutingConfiguration(new List<Type>(), new List<Type>()))))
            {
                routeBuilder.MapVerb(route.Verb, route.Template, route.Handler);
            }

            applicationBuilder.UseRouter(routeBuilder.Build());

            return applicationBuilder;
        }
    }
}