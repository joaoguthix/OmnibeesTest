using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration.Dapper
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetReadConnection()
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("ReadConnection"));
            connection.Open();
            return connection;
        }

        public IDbConnection GetWriteConnection()
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("WriteConnection"));
            connection.Open();
            return connection;
        }
    }
}
