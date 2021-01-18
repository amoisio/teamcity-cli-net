using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CliFx;

namespace TeamCityCliNet
{
    public class Printer
    {
        private readonly IConsole _console;

        public Printer(IConsole console)
        {
            _console = console;
        }

        public void PrintAsList<T>(IEnumerable<T> items, params string[] fields)
        {
            var props = PrintableProperties<T>(fields);
            PrintListHeader(props);
            PrintListRows(items, props);
        }

        public void PrintAsItem<T>(T item, params string[] fields)
        {
            var props = PrintableProperties<T>(fields);
            PrintItemRows(item, props);
        }

        private void PrintItemRows<T>(T item, PropertyInfo[] props)
        {
            foreach (var prop in props)
            {
                var value = prop.GetValue(item)?.ToString();
                _console.Output.WriteLine($"{prop.Name,-10}: {value}");
            }
        }

        private void PrintListHeader(PropertyInfo[] props)
        {
            var headers = props.Select(prop => prop.Name).ToArray();
            PrintListRow(headers);
        }

        private void PrintListRows<T>(IEnumerable<T> items, PropertyInfo[] props)
        {
            foreach (var item in items)
            {
                var values = props.Select(prop => prop.GetValue(item)?.ToString()).ToArray();
                PrintListRow(values);
            }
        }

        private void PrintListRow(string[] items)
        {
            int len = items.Length;
            _console.Output.Write($"{items[0],-10}");
            for (int i = 1; i < len; i++)
                _console.Output.Write($"| {items[i],-10}");

            _console.Output.WriteLine();
        }

        private PropertyInfo[] PrintableProperties<T>(string[] fields)
        {
            var props = new List<PropertyInfo>();
            var allProps = AllPrintablePropertiesOf<T>();
            if (fields != null && fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    var prop = allProps.SingleOrDefault(prop => 
                        String.Equals(field, prop.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (prop != null)
                        props.Add(prop);
                }
            }
            else
            {
                props.AddRange(allProps);
            }
            return props.ToArray();
        }

        private IEnumerable<PropertyInfo> AllPrintablePropertiesOf<T>()
        {
            return PublicPropertiesOf<T>()
                .Where(prop => prop.PropertyType.IsPrimitive
                || prop.PropertyType.IsEnum
                || prop.PropertyType == typeof(string)
                || prop.PropertyType.IsValueType);
        }

        private IEnumerable<PropertyInfo> PublicPropertiesOf<T>()
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