using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace CAF.EFRepository.Helpers
{
    public static class EFHelper
    {
        public static List<T> RawSqlQuery<T>(DbContext context, string query, Func<DbDataReader, T> map)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
        }
        public static DataTable RawSqlQuery(DbContext context, string query)
        {
            var dt = new DataTable();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = query;
                command.CommandType = CommandType.Text;

                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
                return dt;
            }
        }

        internal static void WriteValuesWithBulkCopy(DataTable table, string connectionString, string dbTableName)
        {
            using (var bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BulkCopyTimeout = 60 * 20; // 20 dakika;
                bulkCopy.DestinationTableName = dbTableName;
                bulkCopy.WriteToServer(table);
            }
        }

    }
}
