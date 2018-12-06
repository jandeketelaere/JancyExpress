using System;
using System.Collections.Generic;
using System.Linq;

namespace JancyExpress.Configuration
{
    public class JancyExpressConfiguration
    {
        public JancyExpressConfiguration(Action<IJancyExpressConfigurationExpression> config) : this(Build(config)) { }

        private JancyExpressConfiguration(JancyExpressConfigurationExpression configurationExpression)
        {
            ValidateOnStartup = configurationExpression.ValidateOnStartup;
            AppUseConfiguration = new JancyExpressAppUseConfiguration(configurationExpression.AppConfiguration.UseConfiguration.HttpHandlerMiddlewareTypes, configurationExpression.AppConfiguration.UseConfiguration.ApiHandlerMiddlewareTypes);
            AppVerbConfigurationList = GetAppVerbConfigurationList(configurationExpression.AppConfiguration).ToList();

            if (ValidateOnStartup)
                Validate();
        }

        public bool ValidateOnStartup { get; }

        public JancyExpressAppUseConfiguration AppUseConfiguration { get; }

        public List<JancyExpressAppVerbConfiguration> AppVerbConfigurationList { get; }

        private static JancyExpressConfigurationExpression Build(Action<IJancyExpressConfigurationExpression> config)
        {
            var expression = new JancyExpressConfigurationExpression();

            config(expression);

            return expression;
        }

        public void Validate()
        {
            //todo: move validation to different class
            //todo: check if types are of correct type => e.g. IApiHandlerMiddleware

            //var errorMessage = $"Configuration for {configuration.Verb} {configuration.Template} failed with the following error:";

            //if (configuration.HttpHandlerType == null)
            //    throw new Exception($"{errorMessage} No HttpHandler configured");

            //if (configuration.ApiHandlerType != null)
            //{
            //    var (HttpHandlerRequestType, HttpHandlerResponseType) = GetRequestResponseType(configuration.HttpHandlerType, typeof(IHttpHandler<,>));
            //    var (ApiHandlerRequestType, ApiHandlerResponseType) = GetRequestResponseType(configuration.ApiHandlerType, typeof(IApiHandler<,>));

            //    if (HttpHandlerRequestType != ApiHandlerRequestType)
            //        throw new Exception($"{errorMessage} HttpHandler request '{HttpHandlerRequestType}' is not the same as ApiHandler request '{ApiHandlerRequestType}'");

            //    if (HttpHandlerResponseType != ApiHandlerResponseType)
            //        throw new Exception($"{errorMessage} HttpHandler response '{HttpHandlerResponseType}' is not the same as ApiHandler response '{ApiHandlerResponseType}'");
            //}
        }

        private IEnumerable<JancyExpressAppVerbConfiguration> GetAppVerbConfigurationList(JancyExpressAppConfigurationExpression appConfiguration)
        {
            foreach(var getConfiguration in appConfiguration.VerbConfigurationList)
            {
                yield return new JancyExpressAppVerbConfiguration(getConfiguration.Verb, getConfiguration.Template, getConfiguration.HttpHandlerType, getConfiguration.ApiHandlerType, getConfiguration.HttpHandlerMiddlewareTypes, getConfiguration.ApiHandlerMiddlewareTypes);
            }
        }
    }

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

    public class JancyExpressAppVerbConfiguration
    {
        public JancyExpressAppVerbConfiguration(JancyExpressAppVerb verb, string template, Type httpHandlerType, Type apiHandlerType, List<Type> httpHandlerMiddlewareTypes, List<Type> apiHandlerMiddlewareTypes)
        {
            Verb = verb;
            Template = template;
            HttpHandlerType = httpHandlerType;
            ApiHandlerType = apiHandlerType;
            HttpHandlerMiddlewareTypes = httpHandlerMiddlewareTypes;
            ApiHandlerMiddlewareTypes = apiHandlerMiddlewareTypes;
        }

        public JancyExpressAppVerb Verb { get; set; }
        public string Template { get; set; }
        public Type HttpHandlerType { get; }
        public Type ApiHandlerType { get; }
        public List<Type> HttpHandlerMiddlewareTypes { get; }
        public List<Type> ApiHandlerMiddlewareTypes { get; }
    }

    public enum JancyExpressAppVerb
    {
        Delete,
        Get,
        Post,
        Put
    }
}