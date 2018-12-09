using JancyExpress.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

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

            var routesGenerator = new JancyExpressRoutesGenerator(configuration);
            
            foreach (var route in routesGenerator.GenerateRoutes())
            {
                routeBuilder.MapVerb(route.Verb, route.Template, route.Handler);
            }

            applicationBuilder.UseRouter(routeBuilder.Build());

            return applicationBuilder;
        }
    }
}