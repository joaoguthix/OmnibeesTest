using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Configuration.Context
{
    public class WriteContextSqlServer : BaseContextSqlServer
    {
        public WriteContextSqlServer(DbContextOptions<WriteContextSqlServer> options) : base(options) { }

        public WriteContextSqlServer(DbConnection connection) : base(
            new DbContextOptionsBuilder<WriteContextSqlServer>()
                .UseSqlServer(connection)
                .Options)
        {
        }
    }

    public class SqlServerDbSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }

}