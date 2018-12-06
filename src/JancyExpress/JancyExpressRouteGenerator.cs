using JancyExpress.Configuration;
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
        JancyExpressRoute GenerateRoute(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory);
    }

    internal class JancyExpressRouteGenerator<TRequest, TResponse> : IJancyExpressRouteGenerator
    {
        public JancyExpressRoute GenerateRoute(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory)
        {
            return new JancyExpressRoute
            {
                Verb = appVerbConfiguration.Verb.ToString(),
                Template = appVerbConfiguration.Template,
                Handler = GetHandlerFunc(appVerbConfiguration, appUseConfiguration, serviceFactory)
            };
        }

        private Func<HttpRequest, HttpResponse, RouteData, Task> GetHandlerFunc(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory)
        {
            return (request, response, routeData) =>
            {
                var apiHandler = GetApiHandler(appVerbConfiguration, appUseConfiguration, serviceFactory);
                var httpHandler = GetHttpHandler(appVerbConfiguration, appUseConfiguration, serviceFactory, request, response, routeData, apiHandler);

                return httpHandler();
            };
        }

        private ApiHandlerDelegate<TRequest, TResponse> GetApiHandler(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory)
        {
            if (appVerbConfiguration.ApiHandlerType == null)
                return (request) => throw new NotImplementedException($"Could not execute ApiHandlerDelegate because no API handler was registered for request '{typeof(TRequest)}' and response '{typeof(TResponse)}'");

            ApiHandlerDelegate<TRequest, TResponse> apiHandler = (request) =>
            {
                var handler = serviceFactory.GetInstance<IApiHandler<TRequest, TResponse>>(appVerbConfiguration.ApiHandlerType);
                return handler.Handle(request);
            };

            var apiHandlerMiddlewares = GetApiHandlerMiddlewares(appVerbConfiguration, serviceFactory)
                .Concat(GetGlobalApiHandlerMiddlewares(appUseConfiguration, serviceFactory));

            foreach (var middleware in apiHandlerMiddlewares)
            {
                var previous = apiHandler;
                apiHandler = (request) => middleware.Handle(request, previous);
            }

            return apiHandler;
        }

        private HttpHandlerDelegate GetHttpHandler(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory, HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<TRequest, TResponse> apiHandler)
        {
            HttpHandlerDelegate httpHandler = () =>
            {
                var handler = serviceFactory.GetInstance<IHttpHandler<TRequest, TResponse>>(appVerbConfiguration.HttpHandlerType);
                return handler.Handle(request, response, routeData, (req) => apiHandler(req));
            };

            var httpHandlerMiddlewares = GetHttpHandlerMiddlewares(appVerbConfiguration, serviceFactory)
                .Concat(GetGlobalHttpHandlerMiddlewares(appUseConfiguration, serviceFactory));

            foreach (var middleware in httpHandlerMiddlewares)
            {
                var previous = httpHandler;
                httpHandler = () => middleware.Handle(request, response, routeData, previous);
            }

            return httpHandler;
        }

        private IEnumerable<IHttpHandlerMiddleware<TRequest, TResponse>> GetGlobalHttpHandlerMiddlewares(JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory)
        {
            foreach (var middlewareType in Enumerable.Reverse(appUseConfiguration.HttpHandlerMiddlewareTypes))
            {
                var type = middlewareType.IsGenericType ? middlewareType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : middlewareType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IHttpHandlerMiddleware<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IHttpHandlerMiddleware<TRequest, TResponse>> GetHttpHandlerMiddlewares(JancyExpressAppVerbConfiguration appVerbConfiguration, ServiceFactory serviceFactory)
        {
            foreach (var middlewareType in Enumerable.Reverse(appVerbConfiguration.HttpHandlerMiddlewareTypes))
            {
                var type = middlewareType.IsGenericType ? middlewareType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : middlewareType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IHttpHandlerMiddleware<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IApiHandlerMiddleware<TRequest, TResponse>> GetApiHandlerMiddlewares(JancyExpressAppVerbConfiguration appVerbConfiguration, ServiceFactory serviceFactory)
        {
            foreach(var middlewareType in Enumerable.Reverse(appVerbConfiguration.ApiHandlerMiddlewareTypes))
            {
                var type = middlewareType.IsGenericType ? middlewareType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : middlewareType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IApiHandlerMiddleware<TRequest, TResponse> obj)
                    yield return obj;
            }
        }

        private IEnumerable<IApiHandlerMiddleware<TRequest, TResponse>> GetGlobalApiHandlerMiddlewares(JancyExpressAppUseConfiguration appUseConfiguration, ServiceFactory serviceFactory)
        {
            foreach (var middlewareType in Enumerable.Reverse(appUseConfiguration.ApiHandlerMiddlewareTypes))
            {
                var type = middlewareType.IsGenericType ? middlewareType.MakeGenericType(typeof(TRequest), typeof(TResponse)) : middlewareType;

                //todo: validation
                if (serviceFactory.GetInstance(type) is IApiHandlerMiddleware<TRequest, TResponse> obj)
                    yield return obj;
            }
        }
    }
}