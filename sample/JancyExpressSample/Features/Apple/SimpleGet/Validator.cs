using JancyExpress;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using JancyExpress.Extensions;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class Validator : IRequestHandlerMiddleware
    {
        public Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> Handle()
        {
            return (request, response, routeData, next) =>
            {
                var name = routeData.As<string>("name");

                if (string.IsNullOrEmpty(name))
                    return response.AsJson(400, new { ErrorMessage = "Name cannot be empty" });

                return next();
            };
        }
    }
}