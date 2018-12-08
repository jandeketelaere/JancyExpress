using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace JancyExpress.Tests
{
    public class Request1 { }
    public class Response1 { }

    public class Request2 { }
    public class Response2 { }

    public class HttpHandler1 : IHttpHandler<Request1, Response1>
    {
        public Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<Request1, Response1> apiHandle) => Task.CompletedTask;
    }

    public class HttpHandler2 : IHttpHandler<Request2, Response2>
    {
        public Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<Request2, Response2> apiHandle) => Task.CompletedTask;
    }

    public class ApiHandler1 : IApiHandler<Request1, Response1>
    {
        public Task<Response1> Handle(Request1 request) => Task.FromResult(new Response1());
    }

    public class ApiHandler2 : IApiHandler<Request2, Response2>
    {
        public Task<Response2> Handle(Request2 request) => Task.FromResult(new Response2());
    }

    public class HttpHandlerMiddleware1 : IHttpHandlerMiddleware
    {
        public Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next) => Task.CompletedTask;
    }

    public class HttpHandlerMiddleware2 : IHttpHandlerMiddleware
    {
        public Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next) => Task.CompletedTask;
    }

    public class ApiHandlerMiddleware1 : IApiHandlerMiddleware<Request1, Response1>
    {
        public Task<Response1> Handle(Request1 request, ApiHandlerDelegate<Request1, Response1> next) => Task.FromResult(new Response1());
    }

    public class ApiHandlerMiddleware2 : IApiHandlerMiddleware<Request2, Response2>
    {
        public Task<Response2> Handle(Request2 request, ApiHandlerDelegate<Request2, Response2> next) => Task.FromResult(new Response2());
    }
}