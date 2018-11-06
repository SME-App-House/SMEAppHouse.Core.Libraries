using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SMEAppHouse.Core.Patterns.EF.Helpers
{
    public static class SqlServerUtil
    {
        public static async Task<List<T>> SqlGetObjectList<T>(string connStr, string sqlQry,
            CommandType sqlCmdType)
            where T : class
        {
            return await SqlGetObjectList<T>(connStr, sqlQry, sqlCmdType, 30, null);
        }

        public static async Task<List<T>> SqlGetObjectList<T>(string connStr, string sqlQry,
            CommandType sqlCmdType, SqlParameter[] sqlQryParams)
            where T : class
        {
            return await SqlGetObjectList<T>(connStr, sqlQry, sqlCmdType, 30, sqlQryParams);
        }

        public static async Task<List<T>> SqlGetObjectList<T>(string connStr, string sqlQry,
            CommandType sqlCmdType, int sqlCmdTimeout, SqlParameter[] sqlQryParams)
            where T : class
        {
            var conn = new SqlConnection(connStr);
            return await SqlGetObjectList<T>(conn, sqlQry, sqlCmdType, sqlCmdTimeout, sqlQryParams);
        }

        public static async Task<List<T>> SqlGetObjectList<T>(SqlConnection sqlConnection, string sqlQry,
            CommandType sqlCmdType)
            where T : class
        {
            return await SqlGetObjectList<T>(sqlConnection, sqlQry, sqlCmdType, 30, null);
        }

        public static async Task<List<T>> SqlGetObjectList<T>(SqlConnection sqlConnection, string sqlQry,
            CommandType sqlCmdType, SqlParameter[] sqlQryParams)
            where T : class
        {
            return await SqlGetObjectList<T>(sqlConnection, sqlQry, sqlCmdType, 30, sqlQryParams);
        }

        public static async Task<List<T>> SqlGetObjectList<T>(SqlConnection sqlConnection, string sqlQry, CommandType sqlCmdType, int sqlCmdTimeout, SqlParameter[] sqlQryParams)
            where T : class
        {
            Func<IDataRecord, T> toExplicitOfT = (dr) =>
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if (obj.GetType().GetProperty(prop.Name) == null)
                        continue;

                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            // used for checking if Type instance is a nullable enum
                            var uType = Nullable.GetUnderlyingType(prop.PropertyType);
                            if (uType != null && uType.IsEnum)
                                prop.SetValue(obj, Enum.Parse(uType, dr[prop.Name].ToString()));
                            else if (prop.PropertyType.IsEnum)
                                prop.SetValue(obj, Enum.Parse(prop.PropertyType, dr[prop.Name].ToString()));
                            else
                                prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        //throw;
                    }
                }
                return obj;
            };

            using (var conn = sqlConnection)
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sqlQry;
                    command.CommandType = sqlCmdType;
                    command.CommandTimeout = sqlCmdTimeout;
                    if (sqlQryParams != null && sqlQryParams.Any())
                        command.Parameters.AddRange(sqlQryParams);

                    conn.Open();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var resultList = new List<T>();
                        while (await reader.ReadAsync())
                            resultList.Add(toExplicitOfT(reader));
                        return resultList;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="sqlQuery"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static List<T> ReadSql<T>(IDbConnection conn, string sqlQuery, Func<IDataRecord, T> selector)
            where T : class
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlQuery;
                cmd.Connection.Open();
                using (var r = cmd.ExecuteReader())
                {
                    var items = new List<T>();
                    while (r.Read())
                        items.Add(selector(r));
                    return items;
                }
            }
        }

        public static List<T> SelectValues<T>(SqlConnection conn, string sqlQry, CommandType sqlCmdType,
            Func<IDataRecord, T> selector)
            where T : IConvertible
        {
            return SelectValues<T>(conn, sqlQry, sqlCmdType, 0, selector, null);
        }

        public static List<T> SelectValues<T>(SqlConnection conn, string sqlQry, CommandType sqlCmdType,
            Func<IDataRecord, T> selector, SqlParameter[] sqlQryParams)
            where T : IConvertible
        {
            return SelectValues<T>(conn, sqlQry, sqlCmdType, 0, selector, sqlQryParams);
        }

        public static List<T> SelectValues<T>(SqlConnection conn, string sqlQry, CommandType sqlCmdType, int sqlCmdTimeout, Func<IDataRecord, T> selector, SqlParameter[] sqlQryParams)
            where T : IConvertible
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlQry;
                cmd.CommandType = sqlCmdType;
                cmd.CommandTimeout = sqlCmdTimeout;
                if (sqlQryParams != null && sqlQryParams.Any())
                    cmd.Parameters.AddRange(sqlQryParams);

                cmd.Connection.Open();

                using (var r = cmd.ExecuteReader())
                {
                    var items = new List<T>();
                    while (r.Read())
                        items.Add(selector(r));
                    return items;
                }
            }
        }

        public static T SelectValue<T>(SqlConnection conn, string sqlQry, CommandType sqlCmdType,
            Func<IDataRecord, T> selector, SqlParameter[] sqlQryParams)
            where T : IConvertible
        {
            var results = SelectValues<T>(conn, sqlQry, sqlCmdType, 0, selector, sqlQryParams);
            return results.Any()
                ? results[0]
                : default(T);
        }

        /// <summary>
        /// https://stackoverflow.com/a/16166658/3796898
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }

        /// <summary>
        /// https://stackoverflow.com/a/16166658/3796898
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            return cols.ToDictionary(col => col, col => reader[(string)col]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string SerializeToJson(SqlDataReader reader)
        {
            var r = Serialize(reader);
            return JsonConvert.SerializeObject(r, Newtonsoft.Json.Formatting.Indented);
        }

    }
}