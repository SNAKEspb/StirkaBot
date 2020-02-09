using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace PgSql
{
    public class PgSqlAdapter
    {
        protected string ConnectionString;

        public PgSqlAdapter(ConnectionOptions options) 
            :this($"User ID={options.User};Password={options.Password};Host={options.Host};Port={options.Port};Database={options.DataBase};Pooling=true;Maximum Pool Size=20; Timeout = 300;")
        {

        }
        public PgSqlAdapter(string connectionString)
        {
            ConnectionString = connectionString;
        }
        protected DbConnection GetConnection() => new NpgsqlConnection(ConnectionString);

        public async Task<T> ExecuteReader<T>(string command, Func<DbDataReader, Task<T>> func) {
            return await ExecuteReader(new NpgsqlCommand(command) {  }, func);
        }

        public async Task<T> ExecuteReader<T>(DbCommand command, Func<DbDataReader, Task<T>> func)
        {
            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
                command.Connection = conn;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    T item = await func(reader as DbDataReader);
                    return item;
                }
            }
        }
        public async Task<T> ExecuteScalar<T>(DbCommand command)
        {
            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
                command.Connection = conn;
                var res = await command.ExecuteScalarAsync();
                return res is T ? (T)res : default(T);
            }
        }
        public async Task ExecuteVoid(DbCommand command)
        {
            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
                command.Connection = conn;
                await command.ExecuteScalarAsync();
            }
        }
    }
}