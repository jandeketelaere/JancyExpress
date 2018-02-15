using JancyExpress;
using JancyExpress.Extensions;
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
                var name = routeData.As<string>("name");

                await response.AsJson(200, new { name });
            };
        }
    }
}