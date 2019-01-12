using JancyExpress.Configuration;
using JancyExpressSample.Middleware.ApiHandler;
using JancyExpressSample.Middleware.HttpHandler;

namespace JancyExpressSample.Features
{
    public class GlobalRouter : JancyExpressGlobalRouter
    {
        public GlobalRouter()
        {
            ConfigureRouter(router =>
            {
                router.All()
                    .WithHttpHandlerMiddleware<ExceptionMiddleware>()
                    .WithHttpHandlerMiddleware<RequestResponseLoggingMiddleware>()
                    .WithApiHandlerMiddleware(typeof(TransactionMiddleware<,>));
            });
        }
    }
}