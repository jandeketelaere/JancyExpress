using JancyExpress.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JancyExpress.Extensions;

namespace JancyExpress
{
    internal interface IJancyExpressRouteGenerator
    {
        JancyExpressRoute GenerateRoute(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration);
    }

    internal class JancyExpressRouteGenerator<TRequest, TResponse> : IJancyExpressRouteGenerator
    {
        public JancyExpressRoute GenerateRoute(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration)
        {
            return new JancyExpressRoute
            {
                Verb = appVerbConfiguration.Verb.ToString(),
                Template = appVerbConfiguration.Template,
                Handler = GetHandlerFunc(appVerbConfiguration, appUseConfiguration)
            };
        }

        private Func<HttpRequest, HttpResponse, RouteData, Task> GetHandlerFunc(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration)
        {
            return (request, response, routeData) =>
            {
                var serviceProvider = (IServiceProvider) request.HttpContext.RequestServices.GetService(typeof(IServiceProvider));
                var apiHandler = GetApiHandler(appVerbConfiguration, appUseConfiguration, serviceProvider);
                var httpHandler = GetHttpHandler(appVerbConfiguration, appUseConfiguration, serviceProvider, request, response, routeData, apiHandler);

                return httpHandler();
            };
        }

        private ApiHandlerDelegate<TRequest, TResponse> GetApiHandler(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, IServiceProvider serviceProvider)
        {
            if (appVerbConfiguration.ApiHandlerType == null)
                return (request) => throw new NotImplementedException($"Could not execute ApiHandlerDelegate because no API handler was registered for request '{typeof(TRequest)}' and response '{typeof(TResponse)}'");

            ApiHandlerDelegate<TRequest, TResponse> apiHandler = (request) =>
            {
                var handler = serviceProvider.GetService<IApiHandler<TRequest, TResponse>>(appVerbConfiguration.ApiHandlerType);
                return handler.Handle(request);
            };

            var apiHandlerMiddlewares = GetApiHandlerMiddlewares(appVerbConfiguration.ApiHandlerMiddlewareTypes, serviceProvider)
                .Concat(GetApiHandlerMiddlewares(appUseConfiguration.ApiHandlerMiddlewareTypes, serviceProvider));

            foreach (var middleware in apiHandlerMiddlewares)
            {
                var previous = apiHandler;
                apiHandler = (request) => middleware.Handle(request, () => previous(request));
            }

            return apiHandler;
        }

        private HttpHandlerDelegate GetHttpHandler(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration, IServiceProvider serviceProvider, HttpRequest request, HttpResponse response, RouteData routeData, ApiHandlerDelegate<TRequest, TResponse> apiHandler)
        {
            HttpHandlerDelegate httpHandler = () =>
            {
                var handler = serviceProvider.GetService<IHttpHandler<TRequest, TResponse>>(appVerbConfiguration.HttpHandlerType);
                return handler.Handle(request, response, routeData, apiHandler);
            };

            var httpHandlerMiddlewares = GetHttpHandlerMiddlewares(appVerbConfiguration.HttpHandlerMiddlewareTypes, serviceProvider)
                .Concat(GetHttpHandlerMiddlewares(appUseConfiguration.HttpHandlerMiddlewareTypes, serviceProvider));

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