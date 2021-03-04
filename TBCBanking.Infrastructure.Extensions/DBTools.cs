using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TBCBanking.Domain.Models.DbExtensions;

namespace TBCBanking.Infrastructure.Extensions
{
    public static class DBTools
    {
        private static readonly Dictionary<Type, DbType> dTypes = new Dictionary<Type, DbType>
        {
            { typeof(string), DbType.String },
            { typeof(long), DbType.Int64 },
            { typeof(int), DbType.Int32 },
            { typeof(DateTime), DbType.Date },
            { typeof(decimal), DbType.Decimal },
            { typeof(double), DbType.Decimal },
            { typeof(float), DbType.Decimal },
            { typeof(bool), DbType.Boolean },
        };

        public static async Task<IEnumerable<TRet>> ProcedureReader<TRet, TData>(this SqlConnection db, string procedureName, TData args) where TRet : new()
        {
            db.Open();
            using (SqlCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.AddParams(args);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    return await reader.ReadRow<TRet>();

                //if (reader.NextResult())
            }

            //cmd.ReadParams(args);
        }

        public static async Task<IEnumerable<TRet>> ProcedureReader<TRet>(this SqlConnection db, string procedureName) where TRet : new()
        {
            db.Open();
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    return await reader.ReadRow<TRet>();
            }
        }

        private static void AddParams<T>(this IDbCommand cmd, T t)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => new { Property = x, Attribute = x.GetCustomAttributes<ProcedureParameterAttribute>().FirstOrDefault() })
                .Where(a => a.Attribute != null);

            foreach (var item in properties)
            {
                object value = item.Property.GetValue(t, null);
                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = "@" + item.Attribute?.Name ?? item.Property.Name;
                param.Value = value ?? DBNull.Value;
                param.Direction = (ParameterDirection)(item.Attribute?.PType ?? ParameterType.Input);
                param.DbType = item.Attribute?.DbType ?? GetDbType(item.Property.PropertyType);
                cmd.Parameters.Add(param);
            }
        }

        private static void ReadParams<T>(this IDbCommand cmd, T t)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => x.GetCustomAttributes<ProcedureParameterAttribute>().FirstOrDefault() is ProcedureParameterAttribute attr ? new { Property = x, Attribute = attr } : null);

            foreach (var item in properties)
            {
                if (item == null) continue;
                if (item.Property.PropertyType == typeof(DataTable)) continue;
                object val = cmd.Parameters[item.Attribute.Name ?? item.Property.Name];
                item.Property.SetValue(t, val);
            }
        }

        private static async Task<IEnumerable<T>> ReadRow<T>(this SqlDataReader reader) where T : new()
        {
            List<T> ts = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (await reader.ReadAsync())
            {
                T t = new T();
                foreach (PropertyInfo item in properties)
                {
                    if (item == null) continue;
                    item.SetValue(t, reader[item.Name]);
                }
                ts.Add(t);
            }
            return ts;
        }

        private static DbType GetDbType(Type bType)
        {
            Type tp = Nullable.GetUnderlyingType(bType) ?? bType;
            return dTypes.ContainsKey(tp) ? dTypes[tp] : DbType.String;
        }
    }
}