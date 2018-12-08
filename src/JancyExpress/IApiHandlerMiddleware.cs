using System.Threading.Tasks;

namespace JancyExpress
{
    public delegate Task<TResponse> ApiHandlerDelegate<TRequest, TResponse>(TRequest request);

    public delegate Task ApiHandlerDelegate<TRequest>(TRequest request);

    public interface IApiHandlerMiddleware<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, ApiHandlerDelegate<TRequest, TResponse> next);
    }

    public interface IApiHandlerMiddleware<TRequest> : IApiHandlerMiddleware<TRequest, Unit> { }
}