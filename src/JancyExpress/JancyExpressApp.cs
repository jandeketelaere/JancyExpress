using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress
{
    //todo: Split JancyExpressConfiguration into API & actual configuration => extension methods for API?
    //todo: use delegate instead of injecting IServiceProvider

    public class JancyExpressApp
    {
        private List<JancyExpressConfiguration> _configurations;
        private readonly IServiceProvider _serviceProvider;
        
        public JancyExpressApp(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configurations = new List<JancyExpressConfiguration>();
        }

        public JancyExpressConfiguration Get(string template)
        {
            var configuration = new JancyExpressConfiguration("GET", template);
            _configurations.Add(configuration);

            return configuration;
        }

        internal IEnumerable<JancyExpressRoute> GenerateRoutes()
        {
            foreach(var configuration in _configurations)
            {
                ValidateConfiguration(configuration);
                yield return GenerateRoute(configuration);
            }
        }

        private void ValidateConfiguration(JancyExpressConfiguration configuration)
        {
            var errorMessage = $"Configuration for {configuration.Verb} {configuration.Template} failed with the following error:";

            if (configuration.HttpHandlerType == null)
                throw new Exception($"{errorMessage} No HttpHandler configured");

            if (configuration.ApiHandlerType != null)
            {
                var (HttpHandlerRequestType, HttpHandlerResponseType) = GetRequestResponseType(configuration.HttpHandlerType, typeof(IHttpHandler<,>));
                var (ApiHandlerRequestType, ApiHandlerResponseType) = GetRequestResponseType(configuration.ApiHandlerType, typeof(IApiHandler<,>));

                if (HttpHandlerRequestType != ApiHandlerRequestType)
                    throw new Exception($"{errorMessage} HttpHandler request '{HttpHandlerRequestType}' is not the same as ApiHandler request '{ApiHandlerRequestType}'");

                if (HttpHandlerResponseType != ApiHandlerResponseType)
                    throw new Exception($"{errorMessage} HttpHandler response '{HttpHandlerResponseType}' is not the same as ApiHandler response '{ApiHandlerResponseType}'");
            }
        }

        private JancyExpressRoute GenerateRoute(JancyExpressConfiguration configuration)
        {
            var (RequestType, ResponseType) = GetRequestResponseType(configuration.HttpHandlerType, typeof(IHttpHandler<,>));

            var routeGeneratorType = typeof(JancyExpressRouteGenerator<,>).MakeGenericType(RequestType, ResponseType);
            var routeGenerator = (IJancyExpressRouteGenerator) Activator.CreateInstance(routeGeneratorType);

            return routeGenerator.GenerateRoute(configuration, _serviceProvider);
        }

        private (Type RequestType, Type ResponseType) GetRequestResponseType(Type type, Type genericType)
        {
            var types = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)
                .SelectMany(i => i.GetGenericArguments())
                .ToList();

            return (types[0], types[1]);
        }
    }
}