using System;

namespace JancyExpress
{
    public class JancyExpressConfigurationApi
    {
        internal JancyExpressConfiguration Configuration { get; private set; }

        public JancyExpressConfigurationApi(string verb, string template)
        {
            Configuration = new JancyExpressConfiguration(verb, template);
        }

        public JancyExpressConfigurationApi WithHttpHandlerDecorator(Type type)
        {
            Configuration.HttpHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressConfigurationApi WithHttpHandlerDecorator<THttpHandlerDecorator>()
        {
            Configuration.HttpHandlerDecoratorTypes.Add(typeof(THttpHandlerDecorator));
            return this;
        }

        public JancyExpressConfigurationApi WithApiHandlerDecorator(Type type)
        {
            Configuration.ApiHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressConfigurationApi WithApiHandlerDecorator<TApiHandlerDecorator>()
        {
            Configuration.ApiHandlerDecoratorTypes.Add(typeof(TApiHandlerDecorator));
            return this;
        }

        public JancyExpressConfigurationApi WithHttpHandler<THttpHandler>()
        {
            Configuration.HttpHandlerType = typeof(THttpHandler);
            return this;
        }

        public JancyExpressConfigurationApi WithApiHandler<TApiHandler>()
        {
            Configuration.ApiHandlerType = typeof(TApiHandler);
            return this;
        }
    }
}