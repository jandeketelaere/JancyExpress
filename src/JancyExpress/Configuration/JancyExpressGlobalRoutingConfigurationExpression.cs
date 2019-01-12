using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressGlobalRoutingConfigurationExpression
    {
        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressGlobalRoutingConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an API handler middleware with interface IApiHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressGlobalRoutingConfigurationExpression WithApiHandlerMiddleware(Type type);
    }

    public class JancyExpressGlobalRoutingConfigurationExpression : IJancyExpressGlobalRoutingConfigurationExpression
    {
        public List<Type> HttpHandlerMiddlewareTypes { get; private set; }
        public List<Type> ApiHandlerMiddlewareTypes { get; private set; }

        public JancyExpressGlobalRoutingConfigurationExpression()
        {
            HttpHandlerMiddlewareTypes = new List<Type>();
            ApiHandlerMiddlewareTypes = new List<Type>();
        }

        public IJancyExpressGlobalRoutingConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>()
        {
            HttpHandlerMiddlewareTypes.Add(typeof(THttpHandlerMiddleware));
            return this;
        }

        public IJancyExpressGlobalRoutingConfigurationExpression WithApiHandlerMiddleware(Type type)
        {
            ApiHandlerMiddlewareTypes.Add(type);
            return this;
        }
    }
}