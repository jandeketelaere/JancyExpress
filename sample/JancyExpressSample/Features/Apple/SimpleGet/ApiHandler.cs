using System.Threading.Tasks;
using JancyExpress;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class ApiHandler : IApiHandler<Request, Response>
    {
        public Task<Response> Handle(Request request)
        {
            var response = new Response
            {
                Name = request.Name
            };

            return Task.FromResult(response);
        }
    }
}