using System;

namespace JancyExpress
{
    public class JancyExpressConfigurationApi
    {
        internal JancyExpressConfiguration Configuration { get; private set; }

        public JancyExpressConfigurationApi(string verb, string template)
        {
            Configuration = new JancyExpressConfiguration(verb, template);
        }

        /// <summary>
        /// Register a type that represents an HTTP handler decorator with interface IHttpHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        public JancyExpressConfigurationApi WithHttpHandlerDecorator(Type type)
        {
            Configuration.HttpHandlerDecoratorTypes.Add(type);
            return this;
        }

        /// <summary>
        /// Register a type that represents an HTTP handler decorator with interface IHttpHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// </summary>
        public JancyExpressConfigurationApi WithHttpHandlerDecorator<THttpHandlerDecorator>()
        {
            Configuration.HttpHandlerDecoratorTypes.Add(typeof(THttpHandlerDecorator));
            return this;
        }

        /// <summary>
        /// Registers API handler decorator with interface IApiHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        public JancyExpressConfigurationApi WithApiHandlerDecorator(Type type)
        {
            Configuration.ApiHandlerDecoratorTypes.Add(type);
            return this;
        }

        /// <summary>
        /// Registers API handler decorator with interface IApiHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// </summary>
        public JancyExpressConfigurationApi WithApiHandlerDecorator<TApiHandlerDecorator>()
        {
            Configuration.ApiHandlerDecoratorTypes.Add(typeof(TApiHandlerDecorator));
            return this;
        }

        /// <summary>
        /// Registers HTTP handler with interface IHttpHandler&lt;TRequest, TResponse&gt; or IHttpHandler&lt;TRequest&gt;.
        /// </summary>
        public JancyExpressConfigurationApi WithHttpHandler<THttpHandler>()
        {
            Configuration.HttpHandlerType = typeof(THttpHandler);
            return this;
        }

        /// <summary>
        /// Registers API handler with interface IApiHandler&lt;TRequest, TResponse&gt; or IApiHandler&lt;TRequest&gt;.
        /// </summary>
        public JancyExpressConfigurationApi WithApiHandler<TApiHandler>()
        {
            Configuration.ApiHandlerType = typeof(TApiHandler);
            return this;
        }
    }
}