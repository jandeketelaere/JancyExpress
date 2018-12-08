using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressAppUseConfigurationExpression
    {
        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressAppUseConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an API handler middleware with interface IApiHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressAppUseConfigurationExpression WithApiHandlerMiddleware(Type type);
    }

    public class JancyExpressAppUseConfigurationExpression : IJancyExpressAppUseConfigurationExpression
    {
        public List<Type> HttpHandlerMiddlewareTypes { get; private set; }
        public List<Type> ApiHandlerMiddlewareTypes { get; private set; }

        public JancyExpressAppUseConfigurationExpression()
        {
            HttpHandlerMiddlewareTypes = new List<Type>();
            ApiHandlerMiddlewareTypes = new List<Type>();
        }

        public IJancyExpressAppUseConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>()
        {
            HttpHandlerMiddlewareTypes.Add(typeof(THttpHandlerMiddleware));
            return this;
        }

        public IJancyExpressAppUseConfigurationExpression WithApiHandlerMiddleware(Type type)
        {
            ApiHandlerMiddlewareTypes.Add(type);
            return this;
        }
    }
}