using System;

namespace JancyExpress.Extensions
{
    internal static class ServiceProviderExtensions
    {
        internal static T GetService<T>(this IServiceProvider serviceProvider, Type type) => (T)serviceProvider.GetService(type);
    }
}