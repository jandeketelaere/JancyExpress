using Microsoft.AspNetCore.Routing;

namespace JancyExpress.Extensions
{
    public static class RouteDataExtensions
    {
        public static T As<T>(this RouteData routeData, string key) => routeData.Values[key].As<T>();
    }
}