using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressRouterExpression
    {
        /// <summary>
        /// Register scoped HTTP and API handler middleware for all routing configured in this router
        /// </summary>
        /// <returns></returns>
        IJancyExpressScopedRoutingConfigurationExpression All();

        /// <summary>
        /// Registers routing for a DELETE request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressRoutingConfigurationExpression Delete(string template);

        /// <summary>
        /// Registers routing for a GET request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressRoutingConfigurationExpression Get(string template);

        /// <summary>
        /// Registers routing for a POST request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressRoutingConfigurationExpression Post(string template);

        /// <summary>
        /// Registers routing for a PUT request
        /// </summary>
        /// <param name="template">The template of the request.</param>
        IJancyExpressRoutingConfigurationExpression Put(string template);
    }

    public class JancyExpressRouterExpression : IJancyExpressRouterExpression
    {
        public JancyExpressScopedRoutingConfigurationExpression ScopedRoutingConfiguration { get; private set; }
        public List<JancyExpressRoutingConfigurationExpression> RoutingConfigurations { get; private set; }

        public JancyExpressRouterExpression()
        {
            ScopedRoutingConfiguration = new JancyExpressScopedRoutingConfigurationExpression();
            RoutingConfigurations = new List<JancyExpressRoutingConfigurationExpression>();
        }

        public IJancyExpressScopedRoutingConfigurationExpression All() => ScopedRoutingConfiguration;

        public IJancyExpressRoutingConfigurationExpression Delete(string template) => Verb(JancyExpressRoutingVerb.Delete, template);

        public IJancyExpressRoutingConfigurationExpression Get(string template) => Verb(JancyExpressRoutingVerb.Get, template);

        public IJancyExpressRoutingConfigurationExpression Post(string template) => Verb(JancyExpressRoutingVerb.Post, template);

        public IJancyExpressRoutingConfigurationExpression Put(string template) => Verb(JancyExpressRoutingVerb.Put, template);

        private IJancyExpressRoutingConfigurationExpression Verb(JancyExpressRoutingVerb verb, string template)
        {
            var routingConfiguration = new JancyExpressRoutingConfigurationExpression(verb, template);
            RoutingConfigurations.Add(routingConfiguration);
            return routingConfiguration;
        }
    }
}