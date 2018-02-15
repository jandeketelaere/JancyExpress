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

        public static IApplicationBuilder UseJancyExpress(this IApplicationBuilder applicationBuilder, Action<JancyExpressApp> action)
        {
            var routeBuilder = new RouteBuilder(applicationBuilder);
            var app = new JancyExpressApp();

            action(app);

            foreach (var route in app.Routes)
            {
                routeBuilder.MapVerb(route.Verb, route.Template, route.Handler);
            }

            applicationBuilder.UseRouter(routeBuilder.Build());

            return applicationBuilder;
        }
    }
}