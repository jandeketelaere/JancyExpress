using JancyExpress.Configuration;

namespace JancyExpressSample.Features.Pear
{
    public class PearRouter : JancyExpressRouter
    {
        public PearRouter()
        {
            ConfigureRouter(router =>
            {
                router.Get("api/pear/simpleget/{name}")
                    .WithHttpHandler<SimpleGet.HttpHandler>();
            });
        }
    }
}