using System.Threading.Tasks;

namespace JancyExpress
{
    public interface IApiHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }

    public interface IApiHandler<TRequest> : IApiHandler<TRequest, Unit>
    {
    }

    //public abstract class AsyncApiHandler<TRequest, TResponse> : IApiHandler<TRequest, TResponse>
    //{
    //    Task<TResponse> IApiHandler<TRequest, TResponse>.Handle(TRequest request) => Handle(request);

    //    protected abstract Task<TResponse> Handle(TRequest request);
    //}

    //public abstract class AsyncApiHandler<TRequest> : IApiHandler<TRequest>
    //{
    //    async Task<Unit> IApiHandler<TRequest, Unit>.Handle(TRequest request)
    //    {
    //        await Handle(request);
    //        return Unit.Value;
    //    }

    //    protected abstract Task Handle(TRequest request);
    //}

    //public abstract class ApiHandler<TRequest, TResponse> : IApiHandler<TRequest, TResponse>
    //{
    //    Task<TResponse> IApiHandler<TRequest, TResponse>.Handle(TRequest request) => Task.FromResult(Handle(request));

    //    protected abstract TResponse Handle(TRequest request);
    //}

    //public abstract class ApiHandler<TRequest> : IApiHandler<TRequest>
    //{
    //    Task<Unit> IApiHandler<TRequest, Unit>.Handle(TRequest request)
    //    {
    //        Handle(request);
    //        return Unit.Task;
    //    }

    //    protected abstract void Handle(TRequest request);
    //}
}