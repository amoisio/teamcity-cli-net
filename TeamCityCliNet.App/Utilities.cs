using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TeamCityCliNet
{
    public static class Utilities
    {
        public static IEnumerable<PropertyInfo> AllPrintablePropertiesOf<T>()
        {
            return PublicPropertiesOf<T>()
                .Where(prop => prop.PropertyType.IsPrimitive
                || prop.PropertyType.IsEnum
                || prop.PropertyType == typeof(string)
                || prop.PropertyType.IsValueType);
        }

        private static IEnumerable<PropertyInfo> PublicPropertiesOf<T>()
        {
            var type = typeof(T);
            var props = new List<PropertyInfo>();
            props.AddRange(type.GetProperties());
            props.AddRange(type.GetInterfaces()
                .SelectMany(i => i.GetProperties()));
            return props;
        }
    }
}