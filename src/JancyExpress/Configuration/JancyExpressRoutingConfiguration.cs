using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public class JancyExpressRoutingConfiguration
    {
        public JancyExpressRoutingConfiguration(JancyExpressRoutingVerb verb, string template, Type httpHandlerType, Type apiHandlerType, List<Type> httpHandlerMiddlewareTypes, List<Type> apiHandlerMiddlewareTypes)
        {
            Verb = verb;
            Template = template;
            HttpHandlerType = httpHandlerType;
            ApiHandlerType = apiHandlerType;
            HttpHandlerMiddlewareTypes = httpHandlerMiddlewareTypes;
            ApiHandlerMiddlewareTypes = apiHandlerMiddlewareTypes;
        }

        public JancyExpressRoutingVerb Verb { get; }
        public string Template { get; }
        public Type HttpHandlerType { get; }
        public Type ApiHandlerType { get; }
        public List<Type> HttpHandlerMiddlewareTypes { get; }
        public List<Type> ApiHandlerMiddlewareTypes { get; }
    }
}