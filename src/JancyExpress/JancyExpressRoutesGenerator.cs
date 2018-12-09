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
            var routeGenerator = GetRouteGenerator(appVerbConfiguration, appUseConfiguration);

            return routeGenerator.GenerateRoute(appVerbConfiguration, appUseConfiguration);
        }

        private IJancyExpressRouteGenerator GetRouteGenerator(JancyExpressAppVerbConfiguration appVerbConfiguration, JancyExpressAppUseConfiguration appUseConfiguration)
        {
            var genericArguments = GetGenericArguments(appVerbConfiguration.HttpHandlerType, typeof(IHttpHandler<,>));

            if (!genericArguments.Any())
                return new JancyExpressRouteGenerator();

            (Type requestType, Type responseType) = (genericArguments[0], genericArguments[1]);

            var routeGeneratorType = typeof(JancyExpressRouteGenerator<,>).MakeGenericType(requestType, responseType);

            return (IJancyExpressRouteGenerator)Activator.CreateInstance(routeGeneratorType);
        }

        private IList<Type> GetGenericArguments(Type type, Type genericType)
        {
            return type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)
                .SelectMany(i => i.GetGenericArguments())
                .ToList();
        }
    }
}