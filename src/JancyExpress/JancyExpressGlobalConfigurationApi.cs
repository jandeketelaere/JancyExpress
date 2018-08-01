using System;

namespace JancyExpress
{
    public class JancyExpressGlobalConfigurationApi
    {
        internal JancyExpressGlobalConfiguration Configuration { get; private set; }

        public JancyExpressGlobalConfigurationApi()
        {
            Configuration = new JancyExpressGlobalConfiguration();
        }

        public JancyExpressGlobalConfigurationApi WithHttpHandlerDecorator(Type type)
        {
            Configuration.HttpHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressGlobalConfigurationApi WithHttpHandlerDecorator<THttpHandlerDecorator>()
        {
            Configuration.HttpHandlerDecoratorTypes.Add(typeof(THttpHandlerDecorator));
            return this;
        }

        public JancyExpressGlobalConfigurationApi WithApiHandlerDecorator(Type type)
        {
            Configuration.ApiHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressGlobalConfigurationApi WithApiHandlerDecorator<TApiHandlerDecorator>()
        {
            Configuration.ApiHandlerDecoratorTypes.Add(typeof(TApiHandlerDecorator));
            return this;
        }
    }
}