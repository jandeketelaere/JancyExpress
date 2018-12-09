using JancyExpress;
using System.Threading.Tasks;

namespace JancyExpressSample.Middleware.ApiHandler
{
    public class TransactionMiddleware<TRequest, TResponse> : IApiHandlerMiddleware<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, ApiHandlerDelegate<TResponse> next)
        {
            //open tran
            return next();
        }
    }
}