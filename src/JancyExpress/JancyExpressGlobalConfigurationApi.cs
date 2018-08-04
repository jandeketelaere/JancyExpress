using System;

namespace JancyExpress
{
    public class JancyExpressGlobalConfigurationApi
    {
        internal JancyExpressGlobalConfiguration Configuration { get; private set; }

        public JancyExpressGlobalConfigurationApi()
        {
            Configuration = new JancyExpressGlobalConfiguration();
        }

        /// <summary>
        /// Registers an HTTP handler decorator with interface IHttpHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        public JancyExpressGlobalConfigurationApi WithHttpHandlerDecorator(Type type)
        {
            Configuration.HttpHandlerDecoratorTypes.Add(type);
            return this;
        }

        /// <summary>
        /// Registers an HTTP handler decorator with interface IHttpHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// </summary>
        public JancyExpressGlobalConfigurationApi WithHttpHandlerDecorator<THttpHandlerDecorator>()
        {
            Configuration.HttpHandlerDecoratorTypes.Add(typeof(THttpHandlerDecorator));
            return this;
        }

        /// <summary>
        /// Registers an API handler decorator with interface IApiHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// Use this to register open generic types.
        /// </summary>
        /// <param name="type">The type to register.</param>
        public JancyExpressGlobalConfigurationApi WithApiHandlerDecorator(Type type)
        {
            Configuration.ApiHandlerDecoratorTypes.Add(type);
            return this;
        }

        /// <summary>
        /// Registers an API handler decorator with interface IApiHandlerDecorator&lt;TRequest, TResponse&gt;.
        /// </summary>
        public JancyExpressGlobalConfigurationApi WithApiHandlerDecorator<TApiHandlerDecorator>()
        {
            Configuration.ApiHandlerDecoratorTypes.Add(typeof(TApiHandlerDecorator));
            return this;
        }
    }
}