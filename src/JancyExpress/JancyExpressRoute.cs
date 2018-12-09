using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace JancyExpress
{
    internal class JancyExpressRoute
    {
        public string Verb { get; set; }
        public string Template { get; set; }
        public Func<HttpRequest, HttpResponse, RouteData, Task> Handler { get; set; }
    }
}