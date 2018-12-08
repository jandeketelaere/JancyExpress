using JancyExpress;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimplePost
{
    public class ApiHandler : IApiHandler<Request>
    {
        public Task<Unit> Handle(Request request)
        {
            //store in DB
            return Unit.Task;
        }
    }
}