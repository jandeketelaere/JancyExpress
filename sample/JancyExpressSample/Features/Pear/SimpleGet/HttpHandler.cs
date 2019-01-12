using JancyExpress;
using JancyExpress.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Pear.SimpleGet
{
    public class HttpHandler : IHttpHandler
    {
        public async Task Handle(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            var name = routeData.As<string>("name");

            await response.WriteJson(200, new
            {
                Name = name
            });
        }
    }
}