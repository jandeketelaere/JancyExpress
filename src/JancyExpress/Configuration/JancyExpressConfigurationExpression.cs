using System;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressConfigurationExpression
    {
        bool ValidateOnStartup { get; set; }
        void App(Action<IJancyExpressAppConfigurationExpression> app);
    }

    public class JancyExpressConfigurationExpression : IJancyExpressConfigurationExpression
    {
        public JancyExpressConfigurationExpression()
        {
            AppConfiguration = new JancyExpressAppConfigurationExpression();
        }

        public bool ValidateOnStartup { get; set; }

        public JancyExpressAppConfigurationExpression AppConfiguration { get; private set; }

        public void App(Action<IJancyExpressAppConfigurationExpression> app)
        {
            var expression = new JancyExpressAppConfigurationExpression();
            app(expression);
            AppConfiguration = expression;
        }
    }
}