using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CliFx;

namespace TeamCityCliNet
{
    public static class TablePrinter
    {
        public static void Print<T>(IConsole console, T[] items, string[] fields, int? count = null)
        {
            var rows = count.HasValue 
                ? items.Take(count.Value).ToArray() 
                : items;

            var properties = PrintableProperties<T>(fields);
            var data = ExtractData(rows, properties);
            var sizes = ColumnSizes(data);
            DrawTable(console, data, sizes);
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

        private static string[][] ExtractData<T>(T[] items, PropertyInfo[] properties)
        {
            if (properties == null || properties.Length == 0)
                throw new ArgumentException("Properties must not be empty.");

            int nitems = items?.Length ?? 0;
            int nprops = properties.Length;
            var table = CreateTable(nitems + 1, nprops);

            for (int j = 0; j < nprops; j++)
            {
                table[0][j] = properties[j].Name;
            }

            for (int i = 0; i < nitems; i++)
            {
                T item = items[i];
                for (int j = 0; j < nprops; j++)
                {
                    table[i + 1][j] = properties[j].GetValue(item)?.ToString();
                }
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

        private static int[] ColumnSizes(string[][] data)
        {
            int nrows = data.Length;
            int ncols = data[0].Length;
            var lengths = new int[ncols];
            for (int j = 0; j < ncols; j++)
            {
                for (int i = 0; i < nrows; i++)
                {
                    lengths[j] = Math.Max(lengths[j], data[i][j]?.Length ?? 0);
                }
            }
            return lengths;
        }

        private static void DrawTable(IConsole console, string[][] data, int[] sizes)
        {
            int nrows = data.Length;
            int ncols = data[0].Length;
            for (int i = 0; i < nrows; i++)
            {
                string pattern = $"{{0, -{sizes[0]}}}";
                console.Output.Write(String.Format(pattern, data[i][0]));
                for (int j = 1; j < ncols; j++)
                {
                    pattern = $" | {{0, -{sizes[j]}}}";
                    console.Output.Write(String.Format(pattern, data[i][j]));
                }
                console.Output.WriteLine();
            }
        }
    }
}