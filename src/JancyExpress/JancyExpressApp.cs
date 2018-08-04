using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress
{
    //todo: reevalute configuration api / configuration
    //todo: use delegate instead of injecting IServiceProvider

    public class JancyExpressApp
    {
        private JancyExpressGlobalConfiguration _globalConfiguration;
        private List<JancyExpressConfiguration> _configurations;
        private readonly IServiceProvider _serviceProvider;
        
        public JancyExpressApp(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _globalConfiguration = new JancyExpressGlobalConfiguration();
            _configurations = new List<JancyExpressConfiguration>();
        }

        /// <summary>
        /// Configures handlers and decorators on a global level.
        /// </summary>
        public JancyExpressGlobalConfigurationApi Use()
        {
            var globalConfigurationApi = new JancyExpressGlobalConfigurationApi();
            _globalConfiguration = globalConfigurationApi.Configuration;

            return globalConfigurationApi;
        }

        /// <summary>
        /// Registers routing for a GET request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        public JancyExpressConfigurationApi Get(string template)
        {
            var configurationApi = new JancyExpressConfigurationApi("GET", template);
            _configurations.Add(configurationApi.Configuration);

            return configurationApi;
        }

        internal IEnumerable<JancyExpressRoute> GenerateRoutes()
        {
            //todo: validate global configuration

            foreach(var configuration in _configurations)
            {
                ValidateConfiguration(configuration);
                yield return GenerateRoute(configuration, _globalConfiguration);
            }
        }

        private void ValidateConfiguration(JancyExpressConfiguration configuration)
        {
            //todo: move validation to different class
            //todo: check if types are of correct type => e.g. IApiHandlerDecorator

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

        private JancyExpressRoute GenerateRoute(JancyExpressConfiguration configuration, JancyExpressGlobalConfiguration globalConfiguration)
        {
            var (RequestType, ResponseType) = GetRequestResponseType(configuration.HttpHandlerType, typeof(IHttpHandler<,>));

            var routeGeneratorType = typeof(JancyExpressRouteGenerator<,>).MakeGenericType(RequestType, ResponseType);
            var routeGenerator = (IJancyExpressRouteGenerator) Activator.CreateInstance(routeGeneratorType);

            return routeGenerator.GenerateRoute(configuration, globalConfiguration, _serviceProvider);
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