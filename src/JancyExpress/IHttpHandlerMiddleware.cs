using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpress
{
    public delegate Task HttpHandlerDelegate();

    public interface IHttpHandlerMiddleware
    {
        Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next);
    }
}