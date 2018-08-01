using System;
using System.Collections.Generic;

namespace JancyExpress
{
    public class JancyExpressGlobalConfiguration
    {
        internal List<Type> HttpHandlerDecoratorTypes { get; set; }
        internal List<Type> ApiHandlerDecoratorTypes { get; set; }

        public JancyExpressGlobalConfiguration()
        {
            HttpHandlerDecoratorTypes = new List<Type>();
            ApiHandlerDecoratorTypes = new List<Type>();
        }
    }
}