using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressRoutingConfigurationExpression
    {
        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressRoutingConfigurationExpression WithHttpHandlerMiddleware(Type type);

        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        IJancyExpressRoutingConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an API handler middleware with interface IApiHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressRoutingConfigurationExpression WithApiHandlerMiddleware(Type type);

        /// <summary>
        /// Registers an API handler middleware with interface IHttpHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        IJancyExpressRoutingConfigurationExpression WithApiHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an HTTP handler with interface IHttpHandler&lt;TRequest, TResponse&gt; or IHttpHandler&lt;TRequest&gt;.
        /// </summary>
        IJancyExpressRoutingConfigurationExpression WithHttpHandler<THttpHandler>();

        /// <summary>
        /// Registers an API handler with interface IApiHandler&lt;TRequest, TResponse&gt; or IApiHandler&lt;TRequest&gt;.
        /// </summary>
        IJancyExpressRoutingConfigurationExpression WithApiHandler<TApiHandler>();
    }

    public class JancyExpressRoutingConfigurationExpression : IJancyExpressRoutingConfigurationExpression
    {
        public JancyExpressRoutingVerb Verb { get; private set; }
        public string Template { get; private set; }
        public Type HttpHandlerType { get; private set; }
        public Type ApiHandlerType { get; private set; }
        public List<Type> HttpHandlerMiddlewareTypes { get; private set; }
        public List<Type> ApiHandlerMiddlewareTypes { get; private set; }

        public JancyExpressRoutingConfigurationExpression(JancyExpressRoutingVerb verb, string template)
        {
            Verb = verb;
            Template = template;
            HttpHandlerMiddlewareTypes = new List<Type>();
            ApiHandlerMiddlewareTypes = new List<Type>();
        }

        public IJancyExpressRoutingConfigurationExpression WithHttpHandlerMiddleware(Type type)
        {
            HttpHandlerMiddlewareTypes.Add(type);
            return this;
        }

        public IJancyExpressRoutingConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>()
        {
            HttpHandlerMiddlewareTypes.Add(typeof(THttpHandlerMiddleware));
            return this;
        }

        public IJancyExpressRoutingConfigurationExpression WithApiHandlerMiddleware(Type type)
        {
            ApiHandlerMiddlewareTypes.Add(type);
            return this;
        }

        public IJancyExpressRoutingConfigurationExpression WithApiHandlerMiddleware<TApiHandlerMiddleware>()
        {
            ApiHandlerMiddlewareTypes.Add(typeof(TApiHandlerMiddleware));
            return this;
        }

        public IJancyExpressRoutingConfigurationExpression WithHttpHandler<THttpHandler>()
        {
            HttpHandlerType = typeof(THttpHandler);
            return this;
        }

        public IJancyExpressRoutingConfigurationExpression WithApiHandler<TApiHandler>()
        {
            ApiHandlerType = typeof(TApiHandler);
            return this;
        }
    }
}