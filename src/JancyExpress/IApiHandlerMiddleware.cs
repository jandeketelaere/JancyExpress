using System.Threading.Tasks;

namespace JancyExpress
{
    public delegate Task<TResponse> ApiHandlerDelegate<TResponse>();

    public interface IApiHandlerMiddleware<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, ApiHandlerDelegate<TResponse> next);
    }

    public interface IApiHandlerMiddleware<TRequest> : IApiHandlerMiddleware<TRequest, Unit> { }
}