using JancyExpress.Configuration;
using JancyExpressSample.Features.Apple.Middleware.HttpHandler;

namespace JancyExpressSample.Features.Apple
{
    public class AppleRouter : JancyExpressRouter
    {
        public AppleRouter()
        {
            ConfigureRouter(router =>
            {
                router.All()
                    .WithHttpHandlerMiddleware<HttpSecurity>();

                router.Get("api/apple/simpleget/{name}")
                    .WithHttpHandler<SimpleGet.HttpHandler>()
                    .WithApiHandlerMiddleware<SimpleGet.Validator>()
                    .WithApiHandler<SimpleGet.ApiHandler>();

                router.Post("api/apple/simplepost")
                    .WithHttpHandler<SimplePost.HttpHandler>()
                    .WithApiHandlerMiddleware<SimplePost.Validator>()
                    .WithApiHandler<SimplePost.ApiHandler>();
            });
        }
    }
}