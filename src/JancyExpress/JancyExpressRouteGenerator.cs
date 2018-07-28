using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JancyExpress
{
    internal interface IJancyExpressRouteGenerator
    {
        JancyExpressRoute GenerateRoute(JancyExpressConfiguration configuration, IServiceProvider serviceProvider);
    }

    internal class JancyExpressRouteGenerator<TRequest, TResponse> : IJancyExpressRouteGenerator
    {
        public JancyExpressRoute GenerateRoute(JancyExpressConfiguration configuration, IServiceProvider serviceProvider)
        {
            return new JancyExpressRoute
            {
                Verb = configuration.Verb,
                Template = configuration.Template,
                Handler = GetHandlerFunc(configuration, serviceProvider)
            };
        }

        private Func<HttpRequest, HttpResponse, RouteData, Task> GetHandlerFunc(JancyExpressConfiguration configuration, IServiceProvider serviceProvider)
        {
            return (request, response, routeData) =>
            {
                var apiHandler = (IApiHandler<TRequest, TResponse>)serviceProvider.GetService(configuration.ApiHandlerType);

                HttpHandlerDelegate<Task> httpHandler = () =>
                {
                    var handler = (IHttpHandler<TRequest, TResponse>)serviceProvider.GetService(configuration.HttpHandlerType);
                    return handler.Handle(request, response, routeData, apiHandler);
                };

                var httpHandlerDecorators = GetHttpHandlerDecorators(configuration, serviceProvider);

                foreach (var decorator in httpHandlerDecorators)
                {
                    var previous = httpHandler;
                    httpHandler = () => decorator.Handle(request, response, routeData, apiHandler, previous);
                }

                return httpHandler();
            };
        }

        private IEnumerable<IHttpHandlerDecorator<TRequest, TResponse>> GetHttpHandlerDecorators(JancyExpressConfiguration configuration, IServiceProvider serviceProvider)
        {
            var types = configuration.HttpHandlerDecoratorTypes.Select(t => t.MakeGenericType(typeof(TRequest), typeof(TResponse))).Reverse();
            
            foreach (var type in types)
            {
                yield return (IHttpHandlerDecorator<TRequest, TResponse>) serviceProvider.GetService(type);
            }
        }
    }
}