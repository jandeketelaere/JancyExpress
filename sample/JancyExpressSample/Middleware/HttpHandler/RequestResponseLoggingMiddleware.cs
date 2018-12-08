using JancyExpress;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.IO;
using JancyExpressSample.Infrastructure;

namespace JancyExpressSample.Middleware.HttpHandler
{
    public class RequestResponseLoggingMiddleware : IHttpHandlerMiddleware
    {
        private readonly IJancyLogger _jancyLogger;

        public RequestResponseLoggingMiddleware(IJancyLogger jancyLogger)
        {
            _jancyLogger = jancyLogger;
        }

        public async Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next)
        {
            _jancyLogger.LogInfo(await GetRequestLogging(httpRequest), "Request", httpRequest.HttpContext.TraceIdentifier);
            
            var originalResponseBodyStream = httpResponse.Body;
            var copyResponseBodyStream = new MemoryStream();
            httpResponse.Body = copyResponseBodyStream;

            await next();

            _jancyLogger.LogInfo(await GetResponseLogging(httpResponse, copyResponseBodyStream, originalResponseBodyStream), "Response", httpResponse.HttpContext.TraceIdentifier);
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