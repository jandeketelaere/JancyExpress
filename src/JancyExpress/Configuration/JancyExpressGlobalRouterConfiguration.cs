namespace JancyExpress.Configuration
{
    public class JancyExpressGlobalRouterConfiguration
    {
        public JancyExpressGlobalRouterConfiguration(JancyExpressGlobalRoutingConfiguration globalRoutingConfiguration)
        {
            GlobalRoutingConfiguration = globalRoutingConfiguration;
        }

        public JancyExpressGlobalRoutingConfiguration GlobalRoutingConfiguration { get; }
    }
}