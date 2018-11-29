using System;

namespace JancyExpress
{
    /// <summary>
    /// Factory method to resolve services
    /// </summary>
    /// <param name="serviceType">Type of service to resolve</param>
    /// <returns></returns>
    public delegate object ServiceFactory(Type serviceType);

    internal static class ServiceFactoryExtensions
    {
        internal static T GetInstance<T>(this ServiceFactory factory) => (T)factory(typeof(T));
        internal static T GetInstance<T>(this ServiceFactory factory, Type type) => (T)factory(type);
        internal static object GetInstance(this ServiceFactory factory, Type type) => factory(type);
    }
}