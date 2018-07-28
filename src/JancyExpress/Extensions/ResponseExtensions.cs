using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace JancyExpress.Extensions
{
    public static class ResponseExtensions
    {
        public static Task WriteJson<T>(this HttpResponse response, int httpStatusCode, T value)
        {
            response.ContentType = "application/json";
            response.StatusCode = httpStatusCode;

            return response.WriteAsync(JsonConvert.SerializeObject(value));
        }
    }
}