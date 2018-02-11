using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace JancyExpress
{
    public interface IRequestHandlerMiddleware
    {
        Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> Handle();
    }
}