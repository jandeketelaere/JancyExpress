namespace JancyExpress.Configuration
{
    public interface IJancyExpressGlobalRouterExpression
    {
        /// <summary>
        /// Register global HTTP and API handler middleware for all routing
        /// </summary>
        /// <returns></returns>
        IJancyExpressGlobalRoutingConfigurationExpression All();
    }

    public class JancyExpressGlobalRouterExpression : IJancyExpressGlobalRouterExpression
    {
        public JancyExpressGlobalRoutingConfigurationExpression GlobalRoutingConfiguration { get; private set; }
        
        public JancyExpressGlobalRouterExpression()
        {
            GlobalRoutingConfiguration = new JancyExpressGlobalRoutingConfigurationExpression();
        }

        public IJancyExpressGlobalRoutingConfigurationExpression All() => GlobalRoutingConfiguration;
    }
}