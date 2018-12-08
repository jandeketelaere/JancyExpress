using JancyExpress;
using JancyExpress.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimplePost
{
    public class HttpHandler : IHttpHandler<Request>
    {
        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<Request, Unit> apiHandle)
        {
            var data = request.As<Request>();

            await apiHandle(data);

            await response.WriteJson(200, data.Value1);
        }
    }
}