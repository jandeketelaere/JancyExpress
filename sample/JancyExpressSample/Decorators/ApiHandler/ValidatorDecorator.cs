using JancyExpress;
using System.Threading.Tasks;

namespace JancyExpressSample.Decorators.ApiHandler
{
    public class ValidatorDecorator<TRequest, TResponse> : IApiHandlerDecorator<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, ApiHandlerDelegate<TRequest, TResponse> next)
        {
            return next(request);
        }
    }
}