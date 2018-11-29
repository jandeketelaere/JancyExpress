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
        JancyExpressRoute GenerateRoute(JancyExpressConfiguration configuration, JancyExpressGlobalConfiguration globalConfiguration, ServiceFactory serviceFactory);
    }

    internal class JancyExpressRouteGenerator<TRequest, TResponse> : IJancyExpressRouteGenerator
    {
        public JancyExpressRoute GenerateRoute(JancyExpressConfiguration configuration, JancyExpressGlobalConfiguration globalConfiguration, ServiceFactory serviceFactory)
        {
            return new JancyExpressRoute
            {
                Verb = configuration.Verb,
                Template = configuration.Template,
                Handler = GetHandlerFunc(configuration, globalConfiguration, serviceFactory)
            };
        }

        private Func<HttpRequest, HttpResponse, RouteData, Task> GetHandlerFunc(JancyExpressConfiguration configuration, JancyExpressGlobalConfiguration globalConfiguration, ServiceFactory serviceFactory)
        {
            return (request, response, routeData) =>
            {
                var apiHandler = GetApiHandler(configuration, globalConfiguration, serviceFactory);
                var httpHandler = GetHttpHandler(configuration, globalConfiguration, serviceFactory, request, response, routeData, apiHandler);

                return httpHandler();
            };
        }

        private ApiHandlerDelegate<TRequest, TResponse> GetApiHandler(JancyExpressConfiguration configuration, JancyExpressGlobalConfiguration globalConfiguration, ServiceFactory serviceFactory)
        {
            if (configuration.ApiHandlerType == null)
                return (request) => throw new NotImplementedException($"Could not execute ApiHandlerDelegate because no API handler was registered for request '{typeof(TRequest)}' and response '{typeof(TResponse)}'");

            ApiHandlerDelegate<TRequest, TResponse> apiHandler = (request) =>
            {
                var handler = serviceFactory.GetInstance<IApiHandler<TRequest, TResponse>>(configuration.ApiHandlerType);
                return handler.Handle(request);
            };

            var apiHandlerDecorators = GetApiHandlerDecorators(configuration, serviceFactory)
                .Concat(GetGlobalApiHandlerDecorators(globalConfiguration, serviceFactory));

            foreach (var decorator in apiHandlerDecorators)
            {
                var previous = apiHandler;
                apiHandler = (request) => decorator.Handle(request, previous);
            }

            return apiHandler;
        }

        private HttpHandlerDelegate GetHttpHandler(JancyExpressConfiguration configuration, JancyExpressGlobalConfiguration globalConfiguration, ServiceFactory serviceFactory, HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<TRequest, TResponse> apiHandler)
        {
            HttpHandlerDelegate httpHandler = () =>
            {
                var handler = serviceFactory.GetInstance<IHttpHandler<TRequest, TResponse>>(configuration.HttpHandlerType);
                return handler.Handle(request, response, routeData, (req) => apiHandler(req));
            };

            var httpHandlerDecorators = GetHttpHandlerDecorators(configuration, serviceFactory)
                .Concat(GetGlobalHttpHandlerDecorators(globalConfiguration, serviceFactory));

            foreach (var decorator in httpHandlerDecorators)
            {
                var previous = httpHandler;
                httpHandler = () => decorator.Handle(request, response, routeData, previous);
            }

            return httpHandler;
        }

        private IEnumerable<IHttpHandlerDecorator<TRequest, TResponse>> GetGlobalHttpHandlerDecorators(JancyExpressGlobalConfiguration globalConfiguration, ServiceFactory serviceFactory)
        {
            foreach (var decoratorType in Enumerable.Reverse(globalConfiguration.HttpHandlerDecoratorTypes))
            {
                var type = decoratorType.IsGenericType ? decoratorType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : decoratorType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IHttpHandlerDecorator<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IHttpHandlerDecorator<TRequest, TResponse>> GetHttpHandlerDecorators(JancyExpressConfiguration configuration, ServiceFactory serviceFactory)
        {
            foreach (var decoratorType in Enumerable.Reverse(configuration.HttpHandlerDecoratorTypes))
            {
                var type = decoratorType.IsGenericType ? decoratorType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : decoratorType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IHttpHandlerDecorator<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IApiHandlerDecorator<TRequest, TResponse>> GetApiHandlerDecorators(JancyExpressConfiguration configuration, ServiceFactory serviceFactory)
        {
            foreach(var decoratorType in Enumerable.Reverse(configuration.ApiHandlerDecoratorTypes))
            {
                var type = decoratorType.IsGenericType ? decoratorType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : decoratorType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IApiHandlerDecorator<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IApiHandlerDecorator<TRequest, TResponse>> GetGlobalApiHandlerDecorators(JancyExpressGlobalConfiguration configuration, ServiceFactory serviceFactory)
        {
            foreach (var decoratorType in Enumerable.Reverse(configuration.ApiHandlerDecoratorTypes))
            {
                var type = decoratorType.IsGenericType ? decoratorType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : decoratorType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IApiHandlerDecorator<TRequest, TResponse> obj)
                    yield return obj;
            }
        }
    }
}