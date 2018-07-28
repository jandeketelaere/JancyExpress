using System.Threading.Tasks;

namespace JancyExpress
{
    public delegate Task<TResponse> ApiHandlerDelegate<TRequest, TResponse>(TRequest request);

    public interface IApiHandlerDecorator<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, ApiHandlerDelegate<TRequest, TResponse> next);
    }
}