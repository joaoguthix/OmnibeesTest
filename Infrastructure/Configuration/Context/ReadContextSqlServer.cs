using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration.Context
{
    public class ReadContextSqlServer : BaseContextSqlServer
    {
        public ReadContextSqlServer(DbContextOptions<ReadContextSqlServer> options) : base(options)
        {
        }

    }
}
