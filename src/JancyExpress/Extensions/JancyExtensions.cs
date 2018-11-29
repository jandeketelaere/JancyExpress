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

        public static IApplicationBuilder UseJancyExpress(this IApplicationBuilder applicationBuilder, ServiceFactory serviceFactory, Action<JancyExpressApp> action)
        {
            var routeBuilder = new RouteBuilder(applicationBuilder);
            var app = new JancyExpressApp(serviceFactory);

            action(app);

            foreach (var route in app.GenerateRoutes())
            {
                routeBuilder.MapVerb(route.Verb, route.Template, route.Handler);
            }

            applicationBuilder.UseRouter(routeBuilder.Build());

            return applicationBuilder;
        }
    }
}