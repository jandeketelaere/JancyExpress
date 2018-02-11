using JancyExpress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class Handler : IRequestHandler
    {
        public Func<HttpRequest, HttpResponse, RouteData, Task> Handle()
        {
            return async (request, response, routeData) =>
            {
                var name = request.HttpContext.GetRouteValue("name") as string;

                await response.ReturnJson(200, new { name });
            };
        }
    }
}