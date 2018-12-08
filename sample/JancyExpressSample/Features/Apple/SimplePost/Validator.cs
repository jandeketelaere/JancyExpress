using JancyExpress;
using System;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimplePost
{
    public class Validator : IApiHandlerMiddleware<Request>
    {
        public Task<Unit> Handle(Request request, ApiHandlerDelegate<Request, Unit> next)
        {
            if (request.Value1 > 100)
                throw new Exception("Value1 cannot be larger than 100");

            return Unit.Task;
        }
    }
}