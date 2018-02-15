using System;

namespace JancyExpress.Extensions
{
    internal static class ConversionExtensions
    {
        public static T As<T>(this object value)
        {
            if (value == null) return default(T);
            
            var type = IsNullableType(typeof(T)) ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T);

            return (T) Convert.ChangeType(value, type);
        }

        private static bool IsNullableType(Type type) => (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
    }
}