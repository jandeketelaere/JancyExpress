using JancyExpress;
using System;
using System.Threading.Tasks;

namespace JancyExpressSample.Features.Apple.SimpleGet
{
    public class Validator : IApiHandlerMiddleware<Request, Response>
    {
        public Task<Response> Handle(Request request, ApiHandlerDelegate<Request, Response> next)
        {
            //todo: no exception
            if (string.IsNullOrEmpty(request.Name))
                throw new Exception("Name should not be empty");

            return next(request);
        }
    }
}