using JancyExpress.Configuration;
using Shouldly;
using System.Linq;
using Xunit;

namespace JancyExpress.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void EmptyConfiguration_Should_Not_Wire_Anything()
        {
            var configuration = new JancyExpressConfiguration(config => {});

            configuration.ValidateOnStartup.ShouldBe(false);
            configuration.AppUseConfiguration.ApiHandlerMiddlewareTypes.ShouldBeEmpty();
            configuration.AppUseConfiguration.HttpHandlerMiddlewareTypes.ShouldBeEmpty();
            configuration.AppVerbConfigurationList.ShouldBeEmpty();
        }

        [Fact]
        public void ValidateOnStartup_Should_Wire_ValidateOnStartup()
        {
            var configuration = new JancyExpressConfiguration(config =>
            {
                config.ValidateOnStartup = true;
            });

            configuration.ValidateOnStartup.ShouldBe(true);
        }

        [Fact]
        public void AppUseConfiguration_Should_Wire_Middleware()
        {
            var configuration = new JancyExpressConfiguration(config =>
            {
                config.App(app =>
                {
                    app.Use()
                        .WithHttpHandlerMiddleware<HttpHandlerMiddleware1>()
                        .WithHttpHandlerMiddleware<HttpHandlerMiddleware2>()
                        .WithApiHandlerMiddleware(typeof(ApiHandlerMiddleware1))
                        .WithApiHandlerMiddleware(typeof(ApiHandlerMiddleware2));
                });
            });
            configuration.AppUseConfiguration.HttpHandlerMiddlewareTypes.ShouldContain(typeof(HttpHandlerMiddleware1));
            configuration.AppUseConfiguration.HttpHandlerMiddlewareTypes.ShouldContain(typeof(HttpHandlerMiddleware2));
            configuration.AppUseConfiguration.ApiHandlerMiddlewareTypes.ShouldContain(typeof(ApiHandlerMiddleware1));
            configuration.AppUseConfiguration.ApiHandlerMiddlewareTypes.ShouldContain(typeof(ApiHandlerMiddleware2));
        }

        [Fact]
        public void AppVerbConfiguration_Should_Wire_Handlers_And_Middleware()
        {
            var configuration = new JancyExpressConfiguration(config =>
            {
                config.App(app =>
                {
                    app.Delete("api/delete")
                        .WithHttpHandlerMiddleware(typeof(HttpHandlerMiddleware1))
                        .WithHttpHandlerMiddleware<HttpHandlerMiddleware2>()
                        .WithHttpHandler<HttpHandler1>()
                        .WithApiHandlerMiddleware(typeof(ApiHandlerMiddleware1))
                        .WithApiHandlerMiddleware<ApiHandlerMiddleware2>()
                        .WithApiHandler<ApiHandler1>();

                    app.Get("api/get")
                        .WithHttpHandlerMiddleware(typeof(HttpHandlerMiddleware1))
                        .WithHttpHandlerMiddleware<HttpHandlerMiddleware2>()
                        .WithApiHandlerMiddleware(typeof(ApiHandlerMiddleware1))
                        .WithApiHandlerMiddleware<ApiHandlerMiddleware2>()
                        .WithHttpHandler<HttpHandler1>()
                        .WithApiHandler<ApiHandler1>();

                    app.Post("api/post")
                        .WithHttpHandlerMiddleware(typeof(HttpHandlerMiddleware1))
                        .WithHttpHandlerMiddleware<HttpHandlerMiddleware2>()
                        .WithApiHandlerMiddleware(typeof(ApiHandlerMiddleware1))
                        .WithApiHandlerMiddleware<ApiHandlerMiddleware2>()
                        .WithHttpHandler<HttpHandler1>()
                        .WithApiHandler<ApiHandler1>();

                    app.Put("api/put")
                        .WithHttpHandlerMiddleware(typeof(HttpHandlerMiddleware1))
                        .WithHttpHandlerMiddleware<HttpHandlerMiddleware2>()
                        .WithApiHandlerMiddleware(typeof(ApiHandlerMiddleware1))
                        .WithApiHandlerMiddleware<ApiHandlerMiddleware2>()
                        .WithHttpHandler<HttpHandler1>()
                        .WithApiHandler<ApiHandler1>();
                });
            });

            var deleteConfiguration = configuration.AppVerbConfigurationList.SingleOrDefault(c => c.Verb == JancyExpressAppVerb.Delete);
            ShouldBeCorrect(JancyExpressAppVerb.Delete, "api/delete", deleteConfiguration);

            var getConfiguration = configuration.AppVerbConfigurationList.SingleOrDefault(c => c.Verb == JancyExpressAppVerb.Get);
            ShouldBeCorrect(JancyExpressAppVerb.Get, "api/get", getConfiguration);

            var postConfiguration = configuration.AppVerbConfigurationList.SingleOrDefault(c => c.Verb == JancyExpressAppVerb.Post);
            ShouldBeCorrect(JancyExpressAppVerb.Post, "api/post", postConfiguration);

            var putConfiguration = configuration.AppVerbConfigurationList.SingleOrDefault(c => c.Verb == JancyExpressAppVerb.Put);
            ShouldBeCorrect(JancyExpressAppVerb.Put, "api/put", putConfiguration);
        }

        private static void ShouldBeCorrect(JancyExpressAppVerb verb, string template, JancyExpressAppVerbConfiguration deleteConfiguration)
        {
            deleteConfiguration.ShouldNotBeNull();
            deleteConfiguration.Verb.ShouldBe(verb);
            deleteConfiguration.Template.ShouldBe(template);
            deleteConfiguration.HttpHandlerType.ShouldBe(typeof(HttpHandler1));
            deleteConfiguration.ApiHandlerType.ShouldBe(typeof(ApiHandler1));
            deleteConfiguration.HttpHandlerMiddlewareTypes.ShouldContain(typeof(HttpHandlerMiddleware1));
            deleteConfiguration.HttpHandlerMiddlewareTypes.ShouldContain(typeof(HttpHandlerMiddleware2));
            deleteConfiguration.ApiHandlerMiddlewareTypes.ShouldContain(typeof(ApiHandlerMiddleware1));
            deleteConfiguration.ApiHandlerMiddlewareTypes.ShouldContain(typeof(ApiHandlerMiddleware2));
        }
    }
}