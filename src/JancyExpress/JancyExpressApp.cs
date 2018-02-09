using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JancyExpress
{
    public class JancyExpressApp
    {
        public List<Route> Routes { get; }

        public JancyExpressApp()
        {
            Routes = new List<Route>();
        }

        public void Get(string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler, params Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>[] middleware)
        {
            var delegateHandler = handler;

            foreach (var mw in middleware)
            {
                var next = delegateHandler;
                delegateHandler = (request, response, routeData) => mw(request, response, routeData, () => next(request, response, routeData));
            }

            Routes.Add(new Route
            {
                Verb = "GET",
                Template = template,
                Handler = delegateHandler
            });
        }

        public void Get(string template, IRequestHandler handler)
        {
            Get(template, handler.Handle());
        }

        public void Get(string template, IRequestHandler handler, params Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>[] middleware)
        {
            Get(template, handler.Handle(), middleware);
        }
    }
}