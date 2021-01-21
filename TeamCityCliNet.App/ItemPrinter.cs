using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CliFx;

namespace TeamCityCliNet
{
    public static class ItemPrinter
    {
        public static void Print<T>(IConsole console, T item, string[] fields)
        {
            var properties = PrintableProperties<T>(fields);
            var data = ExtractData(item, properties);
            var size = ColumnSize(data);
            DrawItem(console, data, size);
        }

        private static PropertyInfo[] PrintableProperties<T>(string[] fields)
        {
            var props = new List<PropertyInfo>();
            var allProps = Utilities.AllPrintablePropertiesOf<T>();
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

        private static string[][] ExtractData<T>(T item, PropertyInfo[] properties)
        {
            if (properties == null || properties.Length == 0)
                throw new ArgumentException("Properties must not be empty.");

            int nitems = 1;
            int nprops = properties.Length;
            var table = CreateTable(nitems + 1, nprops);

            for (int j = 0; j < nprops; j++)
            {
                table[0][j] = properties[j].Name;
            }

            for (int j = 0; j < nprops; j++)
            {
                table[1][j] = properties[j].GetValue(item)?.ToString();
            }

            return table;
        }
        
        private static string[][] CreateTable(int rows, int cols)
        {
            var table = new string[rows][];
            for (int i = 0; i < rows;i++)
            {
                table[i] = new string[cols];
            }
            return table;
        }

        private static int ColumnSize(string[][] data)
        {
            var headers = data[0];
            int nheaders = headers.Length;
            int len = 0;
            for (int i = 0; i < nheaders; i++)
            {
                len = Math.Max(len, data[0][i]?.Length ?? 0);
            }
            return len;
        }

        private static void DrawItem(IConsole console, string[][] data, int size)
        {
            int nrows = data.Length;
            for (int i = 0; i < nrows; i++)
            {
                string pattern = $"{{0, -{size}}} : {{1}}";
                console.Output.WriteLine(String.Format(pattern, data[0][i], data[1][i]));
            }
        }
    }
}