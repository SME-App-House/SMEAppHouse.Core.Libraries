using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SMEAppHouse.Core.CodeKits.Extensions
{
    public static class DataExt
    {
        public static string ToCSV(this DataTable table, string delimator)
        {
            var result = new StringBuilder();
            for (var i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
            }
            foreach (DataRow row in table.Rows)
            {
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
                }
            }
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
            //return result.ToString();
        }

        public static string ToString(this Dictionary<string, string> source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source can not be null.");

            var pairs = source.Select(x => $"{x.Key}{keyValueSeparator}{x.Value}").ToArray();

            return string.Join(sequenceSeparator, pairs);
        }
    }
}
