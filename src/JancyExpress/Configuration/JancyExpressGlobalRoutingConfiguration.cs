using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public class JancyExpressGlobalRoutingConfiguration
    {
        public JancyExpressGlobalRoutingConfiguration(List<Type> httpHandlerMiddlewareTypes, List<Type> apiHandlerMiddlewareTypes)
        {
            HttpHandlerMiddlewareTypes = httpHandlerMiddlewareTypes;
            ApiHandlerMiddlewareTypes = apiHandlerMiddlewareTypes;
        }

        public List<Type> HttpHandlerMiddlewareTypes { get; }
        public List<Type> ApiHandlerMiddlewareTypes { get; }
    }
}