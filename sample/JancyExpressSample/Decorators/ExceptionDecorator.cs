using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Net;
using JancyExpress;
using JancyExpressSample.Infrastructure;

namespace JancyExpressSample.Decorators
{
    public class ExceptionDecorator<TRequest, TResponse> : IHttpHandlerDecorator<TRequest, TResponse>
    {
        private readonly IJancyLogger _logger;

        public ExceptionDecorator(IJancyLogger logger)
        {
            _logger = logger;
        }

        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, IApiHandler<TRequest, TResponse> apiHandler, HttpHandlerDelegate<Task> next)
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
        }
    }
}