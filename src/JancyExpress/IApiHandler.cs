using System.Threading.Tasks;

namespace JancyExpress
{
    public interface IApiHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }

    public interface IApiHandler<TRequest> : IApiHandler<TRequest, Unit> {}
}