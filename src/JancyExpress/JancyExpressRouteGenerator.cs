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
        where TRequest : class
        where TResponse : class
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
                var apiHandler = GetApiHandler(configuration, serviceProvider);
                var httpHandler = GetHttpHandler(configuration, serviceProvider, request, response, routeData, apiHandler);

                return httpHandler();
            };
        }

        private ApiHandlerDelegate<TRequest, TResponse> GetApiHandler(JancyExpressConfiguration configuration, IServiceProvider serviceProvider)
        {
            if (configuration.ApiHandlerType == null)
                return (request) => Task.FromResult<TResponse>(null);

            ApiHandlerDelegate<TRequest, TResponse> apiHandler = (request) =>
            {
                var handler = (IApiHandler<TRequest, TResponse>)serviceProvider.GetService(configuration.ApiHandlerType);
                return handler.Handle(request);
            };

            var apiHandlerDecorators = GetApiHandlerDecorators(configuration, serviceProvider);

            foreach (var decorator in apiHandlerDecorators)
            {
                var previous = apiHandler;
                apiHandler = (request) => decorator.Handle(request, previous);
            }

            return apiHandler;
        }

        private HttpHandlerDelegate GetHttpHandler(JancyExpressConfiguration configuration, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<TRequest, TResponse> apiHandler)
        {
            HttpHandlerDelegate httpHandler = () =>
            {
                var handler = (IHttpHandler<TRequest, TResponse>)serviceProvider.GetService(configuration.HttpHandlerType);
                return handler.Handle(request, response, routeData, (req) => apiHandler(req));
            };

            var httpHandlerDecorators = GetHttpHandlerDecorators(configuration, serviceProvider);

            foreach (var decorator in httpHandlerDecorators)
            {
                var previous = httpHandler;
                httpHandler = () => decorator.Handle(request, response, routeData, previous);
            }

            return httpHandler;
        }

        private IEnumerable<IHttpHandlerDecorator<TRequest, TResponse>> GetHttpHandlerDecorators(JancyExpressConfiguration configuration, IServiceProvider serviceProvider)
        {
            foreach (var decoratorType in Enumerable.Reverse(configuration.HttpHandlerDecoratorTypes))
            {
                var type = decoratorType.IsGenericType ? decoratorType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : decoratorType;

                //todo: validation
                if (serviceProvider.GetService(type) is IHttpHandlerDecorator<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IApiHandlerDecorator<TRequest, TResponse>> GetApiHandlerDecorators(JancyExpressConfiguration configuration, IServiceProvider serviceProvider)
        {
            foreach(var decoratorType in Enumerable.Reverse(configuration.ApiHandlerDecoratorTypes))
            {
                var type = decoratorType.IsGenericType ? decoratorType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : decoratorType;

                //todo: validation
                if (serviceProvider.GetService(type) is IApiHandlerDecorator<TRequest, TResponse> obj)
                    yield return obj;
            }
        }
    }
}