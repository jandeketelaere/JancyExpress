using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpress
{
    public delegate Task<TResponse> ApiHandlerDelegate<TRequest, TResponse>(TRequest request);

    public interface IHttpHandler<TRequest, TResponse>
    {
        Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<TRequest, TResponse> apiHandle);
    }

    public interface IHttpHandler<TRequest> : IHttpHandler<TRequest, Unit> { }

    public interface IHttpHandler
    {
        Task Handle(HttpRequest request, HttpResponse response, RouteData routeData);
    }
}