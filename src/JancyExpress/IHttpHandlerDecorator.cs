using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpress
{
    public delegate Task HttpHandlerDelegate<Task>();

    public interface IHttpHandlerDecorator<TRequest, TResponse>
    {
        Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, IApiHandler<TRequest, TResponse> apiHandler, HttpHandlerDelegate<Task> next);
    }
}