using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpress
{
    public interface IHttpHandler<TRequest, TResponse>
    {
        Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, IApiHandler<TRequest, TResponse> apiHandler);
    }

    public interface IHttpHandler<TRequest> : IHttpHandler<TRequest, Unit>
    {
    }

    //public abstract class AsyncHttpHandler<TRequest, TResponse> : IHttpHandler<TRequest, TResponse>
    //{
    //    Task IHttpHandler<TRequest, TResponse>.Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, IApiHandler<TRequest, TResponse> apiHandler) => Handle(httpRequest, httpResponse, routeData, apiHandler);

    //    protected abstract Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, IApiHandler<TRequest, TResponse> apiHandler);
    //}

    //public abstract class AsyncHttpHandler<TRequest> : IHttpHandler<TRequest>
    //{
    //    Task IHttpHandler<TRequest, Unit>.Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, IApiHandler<TRequest, Unit> apiHandler) => Handle(httpRequest, httpResponse, routeData, apiHandler);

    //    protected abstract Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, IApiHandler<TRequest, Unit> apiHandler);
    //}
}