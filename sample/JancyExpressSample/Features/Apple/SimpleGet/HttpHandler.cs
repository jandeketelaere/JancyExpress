using JancyExpress;
using JancyExpress.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class HttpHandler : IHttpHandler<Request, Response>
    {
        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData, IApiHandler<Request, Response> apiHandler)
        {
            var name = routeData.As<string>("name");

            var result = await apiHandler.Handle(new Request
            {
                Name = name
            });

            await response.WriteJson(200, result);
        }
    }
}