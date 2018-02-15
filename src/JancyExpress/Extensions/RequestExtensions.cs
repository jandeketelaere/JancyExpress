using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace JancyExpress.Extensions
{
    public static class RequestExtensions
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public static T As<T>(this HttpRequest request)
        {
            using (var streamReader = new StreamReader(request.Body))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return JsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}