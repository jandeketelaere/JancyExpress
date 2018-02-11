using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace JancyExpress
{
    public static class Extensions
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

        public static Task ReturnJson<T>(this HttpResponse response, int httpStatusCode, T value)
        {
            response.ContentType = "application/json";
            response.StatusCode = httpStatusCode;

            return response.WriteAsync(JsonConvert.SerializeObject(value));
        }
    }
}