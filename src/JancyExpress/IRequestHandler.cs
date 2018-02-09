using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace JancyExpress
{
    public interface IRequestHandler
    {
        Func<HttpRequest, HttpResponse, RouteData, Task> Handle();
    }
}