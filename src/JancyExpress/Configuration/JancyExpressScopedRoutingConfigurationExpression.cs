using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressScopedRoutingConfigurationExpression
    {
        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressScopedRoutingConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an API handler middleware with interface IApiHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressScopedRoutingConfigurationExpression WithApiHandlerMiddleware(Type type);
    }

    public class JancyExpressScopedRoutingConfigurationExpression : IJancyExpressScopedRoutingConfigurationExpression
    {
        public List<Type> HttpHandlerMiddlewareTypes { get; private set; }
        public List<Type> ApiHandlerMiddlewareTypes { get; private set; }

        public JancyExpressScopedRoutingConfigurationExpression()
        {
            HttpHandlerMiddlewareTypes = new List<Type>();
            ApiHandlerMiddlewareTypes = new List<Type>();
        }

        public IJancyExpressScopedRoutingConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>()
        {
            HttpHandlerMiddlewareTypes.Add(typeof(THttpHandlerMiddleware));
            return this;
        }

        public IJancyExpressScopedRoutingConfigurationExpression WithApiHandlerMiddleware(Type type)
        {
            ApiHandlerMiddlewareTypes.Add(type);
            return this;
        }
    }
}