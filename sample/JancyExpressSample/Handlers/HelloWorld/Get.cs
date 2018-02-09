using JancyExpress;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace JancyExpressSample.Handlers.HelloWorld
{
    public class Get : IRequestHandler
    {
        public Func<HttpRequest, HttpResponse, RouteData, Task> Handle()
        {
            return (request, response, routeData) =>
            {
                var name = request.HttpContext.GetRouteValue("name");
                //response.ContentType = "application/json";
                return response.WriteAsync(JsonConvert.SerializeObject(new { name = name }));
            };
        }
    }
}