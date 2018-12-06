using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public class JancyExpressAppVerbConfiguration
    {
        public JancyExpressAppVerbConfiguration(JancyExpressAppVerb verb, string template, Type httpHandlerType, Type apiHandlerType, List<Type> httpHandlerMiddlewareTypes, List<Type> apiHandlerMiddlewareTypes)
        {
            Verb = verb;
            Template = template;
            HttpHandlerType = httpHandlerType;
            ApiHandlerType = apiHandlerType;
            HttpHandlerMiddlewareTypes = httpHandlerMiddlewareTypes;
            ApiHandlerMiddlewareTypes = apiHandlerMiddlewareTypes;
        }

        public JancyExpressAppVerb Verb { get; set; }
        public string Template { get; set; }
        public Type HttpHandlerType { get; }
        public Type ApiHandlerType { get; }
        public List<Type> HttpHandlerMiddlewareTypes { get; }
        public List<Type> ApiHandlerMiddlewareTypes { get; }
    }
}