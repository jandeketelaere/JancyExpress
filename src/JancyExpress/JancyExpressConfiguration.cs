using System;
using System.Collections.Generic;

namespace JancyExpress
{
    //todo: check if types are of correct type => e.g. IApiHandlerDecorator
    public class JancyExpressConfiguration
    {
        internal string Verb { get; }
        internal string Template { get; }

        internal List<Type> HttpHandlerDecoratorTypes { get; private set; }
        internal List<Type> ApiHandlerDecoratorTypes { get; private set; }
        internal Type HttpHandlerType { get; private set; }
        internal Type ApiHandlerType { get; private set; }

        public JancyExpressConfiguration(string verb, string template)
        {
            Verb = verb;
            Template = template;
            HttpHandlerDecoratorTypes = new List<Type>();
            ApiHandlerDecoratorTypes = new List<Type>();
        }

        public JancyExpressConfiguration WithHttpHandlerDecorator(Type type)
        {
            HttpHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressConfiguration WithHttpHandlerDecorator<THttpHandlerDecorator>()
        {
            HttpHandlerDecoratorTypes.Add(typeof(THttpHandlerDecorator));
            return this;
        }

        public JancyExpressConfiguration WithApiHandlerDecorator(Type type)
        {
            ApiHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressConfiguration WithApiHandlerDecorator<TApiHandlerDecorator>()
        {
            ApiHandlerDecoratorTypes.Add(typeof(TApiHandlerDecorator));
            return this;
        }

        public JancyExpressConfiguration WithHttpHandler<THttpHandler>()
        {
            HttpHandlerType = typeof(THttpHandler);
            return this;
        }

        public JancyExpressConfiguration WithApiHandler<TApiHandler>()
        {
            ApiHandlerType = typeof(TApiHandler);
            return this;
        }
    }
}