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
        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<Request1, Response1> apiHandle) {}
    }

    public class HttpHandler2 : IHttpHandler<Request2, Response2>
    {
        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<Request2, Response2> apiHandle) { }
    }

    public class ApiHandler1 : IApiHandler<Request1, Response1>
    {
        public async Task<Response1> Handle(Request1 request) => new Response1();
    }

    public class ApiHandler2 : IApiHandler<Request2, Response2>
    {
        public async Task<Response2> Handle(Request2 request) => new Response2();
    }

    public class HttpHandlerMiddleware1 : IHttpHandlerMiddleware<Request1, Response1>
    {
        public async Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next) {}
    }

    public class HttpHandlerMiddleware2 : IHttpHandlerMiddleware<Request2, Response2>
    {
        public async Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next) {}
    }

    public class ApiHandlerMiddleware1 : IApiHandlerMiddleware<Request1, Response1>
    {
        public async Task<Response1> Handle(Request1 request, ApiHandlerDelegate<Request1, Response1> next) => new Response1();
    }

    public class ApiHandlerMiddleware2 : IApiHandlerMiddleware<Request2, Response2>
    {
        public async Task<Response2> Handle(Request2 request, ApiHandlerDelegate<Request2, Response2> next) => new Response2();
    }
}