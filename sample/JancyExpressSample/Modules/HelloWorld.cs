using JancyExpress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace JancyExpressSample.Modules
{
    public class HelloWorld : JancyModule
    {
        public HelloWorld()
        {
            Get("api/hello/{name}", (request, response, routeData) =>
            {
                var name = request.HttpContext.GetRouteValue("name");
                response.ContentType = "application/json";
                return response.WriteAsync(JsonConvert.SerializeObject(new { name = name }));
            });
        }
    }
}