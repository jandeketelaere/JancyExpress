using JancyExpress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class HttpSecurity : IHttpHandlerDecorator<Request, Response>
    {
        public Task Handle(HttpRequest httpRequest, HttpResponse httpResponse, RouteData routeData, HttpHandlerDelegate next)
        {
            //do some security related stuff on http level
            return next();
        }
    }
}