using JancyExpress;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimplePost
{
    public class ApiHandler : IApiHandler<Request, int>
    {
        public Task<int> Handle(Request request)
        {
            return Task.FromResult(request.Value1 + request.Value2);
        }
    }
}