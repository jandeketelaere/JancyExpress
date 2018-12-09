using JancyExpress.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress
{
    internal class JancyExpressRoutesGenerator
    {
        private readonly JancyExpressConfiguration _configuration;

        internal JancyExpressRoutesGenerator(JancyExpressConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal IEnumerable<JancyExpressRoute> GenerateRoutes()
        {
            foreach(var appVerbConfiguration in _configuration.AppVerbConfigurationList)
            {
                yield return GenerateRoute(appVerbConfiguration, _configuration.AppUseConfiguration);
            }
        }

        private JancyExpressRoute GenerateRoute(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration)
        {
            var (RequestType, ResponseType) = GetRequestResponseType(appVerbConfiguration.HttpHandlerType, typeof(IHttpHandler<,>));

            var routeGeneratorType = typeof(JancyExpressRouteGenerator<,>).MakeGenericType(RequestType, ResponseType);
            var routeGenerator = (IJancyExpressRouteGenerator)Activator.CreateInstance(routeGeneratorType);

            return routeGenerator.GenerateRoute(appVerbConfiguration, appUseConfiguration);
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