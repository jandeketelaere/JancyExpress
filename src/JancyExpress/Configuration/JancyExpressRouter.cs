using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress.Configuration
{
    internal interface IJancyExpressRouter
    {
        JancyExpressRouterConfiguration GetConfiguration();
    }

    public abstract class JancyExpressRouter : IJancyExpressRouter
    {
        private readonly JancyExpressRouterExpression _jancyExpressRouterExpression;

        protected JancyExpressRouter()
        {
            _jancyExpressRouterExpression = new JancyExpressRouterExpression();
        }

        protected void ConfigureRouter(Action<IJancyExpressRouterExpression> router)
        {
            router(_jancyExpressRouterExpression);
        }

        JancyExpressRouterConfiguration IJancyExpressRouter.GetConfiguration()
        {
            return new JancyExpressRouterConfiguration(GetRoutingConfigurations().ToList(), GetScopedRoutingConfiguration());
        }

        private IEnumerable<JancyExpressRoutingConfiguration> GetRoutingConfigurations()
        {
            foreach(var configuration in _jancyExpressRouterExpression.RoutingConfigurations)
            {
                yield return new JancyExpressRoutingConfiguration(configuration.Verb, configuration.Template, configuration.HttpHandlerType, configuration.ApiHandlerType, configuration.HttpHandlerMiddlewareTypes, configuration.ApiHandlerMiddlewareTypes);
            }
        }

        private JancyExpressScopedRoutingConfiguration GetScopedRoutingConfiguration()
        {
            return new JancyExpressScopedRoutingConfiguration(_jancyExpressRouterExpression.ScopedRoutingConfiguration.HttpHandlerMiddlewareTypes, _jancyExpressRouterExpression.ScopedRoutingConfiguration.ApiHandlerMiddlewareTypes);
        }
    }
}