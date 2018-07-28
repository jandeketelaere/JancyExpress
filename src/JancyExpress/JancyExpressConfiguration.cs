using System;
using System.Collections.Generic;

namespace JancyExpress
{
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

        public JancyExpressConfiguration WithApiHandlerDecorator(Type type)
        {
            ApiHandlerDecoratorTypes.Add(type);
            return this;
        }

        public JancyExpressConfiguration WithHttpHandler<IHttpHandler>()
        {
            HttpHandlerType = typeof(IHttpHandler);
            return this;
        }

        public JancyExpressConfiguration WithApiHandler<IApiHandler>()
        {
            ApiHandlerType = typeof(IApiHandler);
            return this;
        }
    }
}