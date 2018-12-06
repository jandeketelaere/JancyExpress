using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net;
using JancyExpress;
using JancyExpressSample.Infrastructure;
using JancyExpress.Extensions;

namespace JancyExpressSample.Middleware.HttpHandler
{
    public class ExceptionMiddleware<TRequest, TResponse> : IHttpHandlerMiddleware<TRequest, TResponse>
    {
        private readonly IJancyLogger _logger;

        public ExceptionMiddleware(IJancyLogger logger)
        {
            _logger = logger;
        }

        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, HttpHandlerDelegate next)
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), "Exception", request.HttpContext.TraceIdentifier);

                await response.WriteJson((int)HttpStatusCode.InternalServerError, new { ErrorMessage = "An unhandled exception occurred." });
            }
        }
    }
}