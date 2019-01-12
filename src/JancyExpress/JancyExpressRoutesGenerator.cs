using JancyExpress.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress
{
    internal class JancyExpressRoutesGenerator
    {
        internal IEnumerable<JancyExpressRoute> GenerateRoutes(List<JancyExpressRouterConfiguration> routerConfigurations, JancyExpressGlobalRouterConfiguration globalRouterConfiguration)
        {
            foreach(var routerConfiguration in routerConfigurations)
            {
                foreach(var routingConfiguration in routerConfiguration.RoutingConfigurations)
                {
                    yield return GenerateRoute(routingConfiguration, routerConfiguration.ScopedRoutingConfiguration, globalRouterConfiguration);
                }
            }
        }

        private JancyExpressRoute GenerateRoute(JancyExpressRoutingConfiguration routingConfiguration, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration, JancyExpressGlobalRouterConfiguration globalRouterConfiguration)
        {
            var routeGenerator = GetRouteGenerator(routingConfiguration.HttpHandlerType);

            return routeGenerator.GenerateRoute(routingConfiguration, scopedRoutingConfiguration, globalRouterConfiguration.GlobalRoutingConfiguration);
        }

        private IJancyExpressRouteGenerator GetRouteGenerator(Type httpHandlerType)
        {
            var genericArguments = GetGenericArguments(httpHandlerType, typeof(IHttpHandler<,>));

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