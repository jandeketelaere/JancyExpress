using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JancyExpress
{
    public abstract class JancyModule
    {
        public List<Route> Routes { get; }

        public JancyModule()
        {
            Routes = new List<Route>();
        }

        public void Get(string template, Func<HttpRequest, HttpResponse, RouteData, Task> handler)
        {
            Routes.Add(new Route
            {
                Verb = "GET",
                Template = template,
                Handler = handler
            });
        }
    }
}