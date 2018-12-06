using System;
using System.Collections.Generic;

namespace JancyExpress.Configuration
{
    public interface IJancyExpressAppVerbConfigurationExpression
    {
        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressAppVerbConfigurationExpression WithHttpHandlerMiddleware(Type type);

        /// <summary>
        /// Registers an HTTP handler middleware with interface IHttpHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        IJancyExpressAppVerbConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an API handler middleware with interface IApiHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        IJancyExpressAppVerbConfigurationExpression WithApiHandlerMiddleware(Type type);

        /// <summary>
        /// Registers an API handler middleware with interface IHttpHandlerMiddleware&lt;TRequest, TResponse&gt;.
        /// </summary>
        IJancyExpressAppVerbConfigurationExpression WithApiHandlerMiddleware<THttpHandlerMiddleware>();

        /// <summary>
        /// Registers an HTTP handler with interface IHttpHandler&lt;TRequest, TResponse&gt; or IHttpHandler&lt;TRequest&gt;.
        /// </summary>
        IJancyExpressAppVerbConfigurationExpression WithHttpHandler<THttpHandler>();

        /// <summary>
        /// Registers an API handler with interface IApiHandler&lt;TRequest, TResponse&gt; or IApiHandler&lt;TRequest&gt;.
        /// </summary>
        IJancyExpressAppVerbConfigurationExpression WithApiHandler<TApiHandler>();
    }

    public class JancyExpressAppVerbConfigurationExpression : IJancyExpressAppVerbConfigurationExpression
    {
        public JancyExpressAppVerb Verb { get; private set; }
        public string Template { get; private set; }
        public Type HttpHandlerType { get; private set; }
        public Type ApiHandlerType { get; private set; }
        public List<Type> HttpHandlerMiddlewareTypes { get; private set; }
        public List<Type> ApiHandlerMiddlewareTypes { get; private set; }

        public JancyExpressAppVerbConfigurationExpression(JancyExpressAppVerb verb, string template)
        {
            Verb = verb;
            Template = template;
            HttpHandlerMiddlewareTypes = new List<Type>();
            ApiHandlerMiddlewareTypes = new List<Type>();
        }

        public IJancyExpressAppVerbConfigurationExpression WithHttpHandlerMiddleware(Type type)
        {
            HttpHandlerMiddlewareTypes.Add(type);
            return this;
        }

        public IJancyExpressAppVerbConfigurationExpression WithHttpHandlerMiddleware<THttpHandlerMiddleware>()
        {
            HttpHandlerMiddlewareTypes.Add(typeof(THttpHandlerMiddleware));
            return this;
        }

        public IJancyExpressAppVerbConfigurationExpression WithApiHandlerMiddleware(Type type)
        {
            ApiHandlerMiddlewareTypes.Add(type);
            return this;
        }

        public IJancyExpressAppVerbConfigurationExpression WithApiHandlerMiddleware<TApiHandlerMiddleware>()
        {
            ApiHandlerMiddlewareTypes.Add(typeof(TApiHandlerMiddleware));
            return this;
        }

        public IJancyExpressAppVerbConfigurationExpression WithHttpHandler<THttpHandler>()
        {
            HttpHandlerType = typeof(THttpHandler);
            return this;
        }

        public IJancyExpressAppVerbConfigurationExpression WithApiHandler<TApiHandler>()
        {
            ApiHandlerType = typeof(TApiHandler);
            return this;
        }
    }
}