using JancyExpress;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class Validator : IRequestHandlerMiddleware
    {
        public Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> Handle()
        {
            return (request, response, routeData, next) =>
            {
                var name = request.HttpContext.GetRouteValue("name") as string;

                if (string.IsNullOrEmpty(name))
                    return response.ReturnJson(400, new { ErrorMessage = "Name cannot be empty" });

                return next();
            };
        }
    }
}