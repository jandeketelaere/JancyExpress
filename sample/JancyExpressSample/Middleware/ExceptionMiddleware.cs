using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Net;
using JancyExpress;
using JancyExpressSample.Infrastructure;

namespace JancyExpressSample.Middleware
{
    public class ExceptionMiddleware : IRequestHandlerMiddleware
    {
        private readonly IJancyLogger _logger;

        public ExceptionMiddleware(IJancyLogger logger)
        {
            _logger = logger;
        }

        public Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> Handle()
        {
            return async (request, response, routeData, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString(), "Exception", request.HttpContext.TraceIdentifier);
                    
                    var result = JsonConvert.SerializeObject(new { ErrorMessage = "An unhandled exception occurred." });
                    response.ContentType = "application/json";
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await response.WriteAsync(result);
                }
            };
        }
    }
}