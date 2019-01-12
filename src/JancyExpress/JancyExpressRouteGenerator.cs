using JancyExpress.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JancyExpress.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace JancyExpress
{
    internal interface IJancyExpressRouteGenerator
    {
        JancyExpressRoute GenerateRoute(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration);
    }

    internal class JancyExpressRouteGenerator : IJancyExpressRouteGenerator
    {
        public JancyExpressRoute GenerateRoute(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration)
        {
            return new JancyExpressRoute
            {
                Verb = routingConfiguration.Verb.ToString(),
                Template = routingConfiguration.Template,
                Handler = GetHandlerFunc(routingConfiguration, scopedRoutingConfiguration, globalRoutingConfiguration)
            };
        }

        private Func<HttpRequest, HttpResponse, RouteData, Task> GetHandlerFunc(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration)
        {
            return (request, response, routeData) =>
            {
                var serviceProvider = request.HttpContext.RequestServices;
                var httpHandler = GetHttpHandler(routingConfiguration, scopedRoutingConfiguration, globalRoutingConfiguration, serviceProvider, request, response, routeData);

                return httpHandler();
            };
        }

        private HttpHandlerDelegate GetHttpHandler(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response, RouteData routeData)
        {
            HttpHandlerDelegate httpHandler = () =>
            {
                var handler = serviceProvider.GetService<IHttpHandler>(routingConfiguration.HttpHandlerType);
                return handler.Handle(request, response, routeData);
            };

            var httpHandlerMiddlewares =
                GetHttpHandlerMiddlewares(routingConfiguration.HttpHandlerMiddlewareTypes, serviceProvider)
                .Concat(GetHttpHandlerMiddlewares(scopedRoutingConfiguration.HttpHandlerMiddlewareTypes, serviceProvider))
                .Concat(GetHttpHandlerMiddlewares(globalRoutingConfiguration.HttpHandlerMiddlewareTypes, serviceProvider));

            foreach (var middleware in httpHandlerMiddlewares)
            {
                var previous = httpHandler;
                httpHandler = () => middleware.Handle(request, response, routeData, previous);
            }

            return httpHandler;
        }

        private IEnumerable<IHttpHandlerMiddleware> GetHttpHandlerMiddlewares(List<Type> middlewareTypes, IServiceProvider serviceProvider)
        {
            foreach (var middlewareType in Enumerable.Reverse(middlewareTypes))
            {
                yield return serviceProvider.GetService<IHttpHandlerMiddleware>(middlewareType);
            }
        }
    }

    internal class JancyExpressRouteGenerator<TRequest, TResponse> : IJancyExpressRouteGenerator
    {
        public JancyExpressRoute GenerateRoute(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration)
        {
            return new JancyExpressRoute
            {
                Verb = routingConfiguration.Verb.ToString(),
                Template = routingConfiguration.Template,
                Handler = GetHandlerFunc(routingConfiguration, scopedRoutingConfiguration, globalRoutingConfiguration)
            };
        }

        private Func<HttpRequest, HttpResponse, RouteData, Task> GetHandlerFunc(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration)
        {
            return (request, response, routeData) =>
            {
                var serviceProvider = request.HttpContext.RequestServices;
                var apiHandler = GetApiHandler(routingConfiguration, scopedRoutingConfiguration, globalRoutingConfiguration, serviceProvider);
                var httpHandler = GetHttpHandler(routingConfiguration, scopedRoutingConfiguration, globalRoutingConfiguration, serviceProvider, request, response, routeData, apiHandler);

                return httpHandler();
            };
        }

        private ApiHandlerDelegate<TRequest, TResponse> GetApiHandler(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration, IServiceProvider serviceProvider)
        {
            if (routingConfiguration.ApiHandlerType == null)
                return (request) => throw new NotImplementedException($"Could not execute ApiHandlerDelegate because no API handler was registered for request '{typeof(TRequest)}' and response '{typeof(TResponse)}'");

            ApiHandlerDelegate<TRequest, TResponse> apiHandler = (request) =>
            {
                var handler = serviceProvider.GetService<IApiHandler<TRequest, TResponse>>(routingConfiguration.ApiHandlerType);
                return handler.Handle(request);
            };

            var apiHandlerMiddlewares =
                GetApiHandlerMiddlewares(routingConfiguration.ApiHandlerMiddlewareTypes, serviceProvider)
                .Concat(GetApiHandlerMiddlewares(scopedRoutingConfiguration.ApiHandlerMiddlewareTypes, serviceProvider))
                .Concat(GetApiHandlerMiddlewares(globalRoutingConfiguration.ApiHandlerMiddlewareTypes, serviceProvider));

            foreach (var middleware in apiHandlerMiddlewares)
            {
                var previous = apiHandler;
                apiHandler = (request) => middleware.Handle(request, () => previous(request));
            }

            return apiHandler;
        }

        private HttpHandlerDelegate GetHttpHandler(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<TRequest, TResponse> apiHandler)
        {
            HttpHandlerDelegate httpHandler = () =>
            {
                var handler = serviceProvider.GetService<IHttpHandler<TRequest, TResponse>>(routingConfiguration.HttpHandlerType);
                return handler.Handle(request, response, routeData, apiHandler);
            };

            var httpHandlerMiddlewares =
                GetHttpHandlerMiddlewares(routingConfiguration.HttpHandlerMiddlewareTypes, serviceProvider)
                .Concat(GetHttpHandlerMiddlewares(scopedRoutingConfiguration.HttpHandlerMiddlewareTypes, serviceProvider))
                .Concat(GetHttpHandlerMiddlewares(globalRoutingConfiguration.HttpHandlerMiddlewareTypes, serviceProvider));

            foreach (var middleware in httpHandlerMiddlewares)
            {
                var previous = httpHandler;
                httpHandler = () => middleware.Handle(request, response, routeData, previous);
            }

            return httpHandler;
        }

        private IEnumerable<IHttpHandlerMiddleware> GetHttpHandlerMiddlewares(List<Type> middlewareTypes, IServiceProvider serviceProvider)
        {
            foreach (var middlewareType in Enumerable.Reverse(middlewareTypes))
            {
                yield return serviceProvider.GetService<IHttpHandlerMiddleware>(middlewareType);
            }
        }

        private IEnumerable<IApiHandlerMiddleware<TRequest, TResponse>> GetApiHandlerMiddlewares(List<Type> middlewareTypes, IServiceProvider serviceProvider)
        {
            foreach (var middlewareType in Enumerable.Reverse(middlewareTypes))
            {
                var type = middlewareType.IsGenericType ? middlewareType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : middlewareType;

                yield return serviceProvider.GetService<IApiHandlerMiddleware<TRequest, TResponse>>(type);
            }
        }
    }
}