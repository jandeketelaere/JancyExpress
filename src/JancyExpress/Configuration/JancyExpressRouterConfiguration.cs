using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public class JancyExpressRouterConfiguration
    {
        public JancyExpressRouterConfiguration(List<JancyExpressRoutingConfiguration> routingConfigurations, JancyExpressScopedRoutingConfiguration scopedRoutingConfiguration)
        {
            RoutingConfigurations = routingConfigurations;
            ScopedRoutingConfiguration = scopedRoutingConfiguration;
        }

        public List<JancyExpressRoutingConfiguration> RoutingConfigurations { get; }
        public JancyExpressScopedRoutingConfiguration ScopedRoutingConfiguration { get; }
    }
}