using System;

namespace JancyExpress.Configuration
{
    internal interface IJancyExpressGlobalRouter
    {
        JancyExpressGlobalRouterConfiguration GetConfiguration();
    }

    public abstract class JancyExpressGlobalRouter : IJancyExpressGlobalRouter
    {
        private readonly JancyExpressGlobalRouterExpression _jancyExpressGlobalRouterExpression;

        protected JancyExpressGlobalRouter()
        {
            _jancyExpressGlobalRouterExpression = new JancyExpressGlobalRouterExpression();
        }

        protected void ConfigureRouter(Action<IJancyExpressGlobalRouterExpression> router)
        {
            router(_jancyExpressGlobalRouterExpression);
        }

        JancyExpressGlobalRouterConfiguration IJancyExpressGlobalRouter.GetConfiguration()
        {
            return new JancyExpressGlobalRouterConfiguration(new JancyExpressGlobalRoutingConfiguration(_jancyExpressGlobalRouterExpression.GlobalRoutingConfiguration.HttpHandlerMiddlewareTypes, _jancyExpressGlobalRouterExpression.GlobalRoutingConfiguration.ApiHandlerMiddlewareTypes));
        }
    }
}