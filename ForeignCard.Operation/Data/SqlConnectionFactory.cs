using System.Data;
using ForeignCard.Common.Abstractions;
using Microsoft.Data.SqlClient;

namespace ForeignCard.Operation.Data
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}