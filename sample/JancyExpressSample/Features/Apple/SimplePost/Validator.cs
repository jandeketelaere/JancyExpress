using JancyExpress;
using System;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimplePost
{
    public class Validator : IApiHandlerMiddleware<Request>
    {
        public async Task<Unit> Handle(Request request, ApiHandlerDelegate<Unit> next)
        {
            //todo: no exception
            if (request.Value1 > 100)
                throw new Exception("Value1 cannot be larger than 100");

            await next();

            return Unit.Value;
        }
    }
}