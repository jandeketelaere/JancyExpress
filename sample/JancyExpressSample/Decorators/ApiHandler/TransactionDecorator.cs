using JancyExpress;
using System.Threading.Tasks;

namespace JancyExpressSample.Decorators.ApiHandler
{
    public class TransactionDecorator<TRequest, TResponse> : IApiHandlerDecorator<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, ApiHandlerDelegate<TRequest, TResponse> next)
        {
            //open tran
            return next(request);
        }
    }
}