using JancyExpress;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.IO;
using JancyExpressSample.Infrastructure;

namespace JancyExpressSample.Middleware
{
    public class RequestResponseLoggingMiddleware : IRequestHandlerMiddleware
    {
        private readonly IJancyLogger _jancyLogger;

        public RequestResponseLoggingMiddleware(IJancyLogger jancyLogger)
        {
            _jancyLogger = jancyLogger;
        }

        public Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> Handle()
        {
            return async (request, response, routeData, next) =>
            {
                _jancyLogger.LogInfo(await GetRequestLogging(request), "Request", request.HttpContext.TraceIdentifier);

                var originalResponseBodyStream = response.Body;
                var copyResponseBodyStream = new MemoryStream();
                response.Body = copyResponseBodyStream;

                await next();

                _jancyLogger.LogInfo(await GetResponseLogging(response, copyResponseBodyStream, originalResponseBodyStream), "Response", response.HttpContext.TraceIdentifier);
            };
        }

        private async Task<string> GetRequestLogging(HttpRequest request)
        {
            var copyRequestBodyStream = new MemoryStream();
            await request.Body.CopyToAsync(copyRequestBodyStream);
            copyRequestBodyStream.Seek(0, SeekOrigin.Begin);

            var body = await new StreamReader(copyRequestBodyStream).ReadToEndAsync();

            copyRequestBodyStream.Seek(0, SeekOrigin.Begin);
            request.Body = copyRequestBodyStream;

            return $"Method: {request.Method}, Url: {request.Path}{(!string.IsNullOrEmpty(body) ? $", Body: {body}" : string.Empty)}, Client: {request.HttpContext.Connection.RemoteIpAddress}";
        }

        private async Task<string> GetResponseLogging(HttpResponse response, MemoryStream responseBodyStream, Stream originalResponseBodyStream)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(responseBodyStream).ReadToEndAsync();

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            response.Body = responseBodyStream;

            if (response.StatusCode != 204)
                await responseBodyStream.CopyToAsync(originalResponseBodyStream);

            return $"Code: {response.StatusCode}{(!string.IsNullOrEmpty(body) ? $", Body: {body}" : string.Empty)}";
        }
    }
}