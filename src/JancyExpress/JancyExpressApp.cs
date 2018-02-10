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
        private readonly List<Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>> _globalMiddleware;

        public JancyExpressApp()
        {
            Routes = new List<Route>();
            _globalMiddleware = new List<Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>>();
        }

        #region Get

        private void Get(string template, List<Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>> middleware, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
        {
            var delegateHandler = handler;
            var extendedMiddleware = middleware.Concat(_globalMiddleware);

            foreach (var mw in extendedMiddleware)
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

        public void Get(string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
        {
            Get(template, new List<Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>>(), handler);
        }

        public void Get(string template, Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> middleware1, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
        {
            Get(template, new List<Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>> { middleware1 }, handler);
        }

        public void Get(string template, Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> middleware1, Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task> middleware2, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
        {
            Get(template, new List<Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>> { middleware2, middleware1 }, handler);
        }

        #endregion

        public void Use(params Func<HttpRequest, HttpResponse, RouteData, Func<Task>, Task>[] middleware)
        {
            foreach(var mw in middleware.Select((m, i) => new { Index = i, Middleware = m}).OrderByDescending(m => m.Index))
            {
                _globalMiddleware.Add(mw.Middleware);
            }
        }
    }
}