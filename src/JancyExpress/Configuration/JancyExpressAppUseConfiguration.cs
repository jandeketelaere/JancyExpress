using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public class JancyExpressAppUseConfiguration
    {
        public JancyExpressAppUseConfiguration(List<Type> httpHandlerMiddlewareTypes, List<Type> apiHandlerMiddlewareTypes)
        {
            HttpHandlerMiddlewareTypes = httpHandlerMiddlewareTypes;
            ApiHandlerMiddlewareTypes = apiHandlerMiddlewareTypes;
        }

        public List<Type> HttpHandlerMiddlewareTypes { get; }
        public List<Type> ApiHandlerMiddlewareTypes { get; }
    }
}