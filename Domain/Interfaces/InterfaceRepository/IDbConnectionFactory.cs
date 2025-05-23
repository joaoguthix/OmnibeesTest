using System.Data;

namespace Infrastructure.Configuration.Dapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetReadConnection();
        IDbConnection GetWriteConnection();
    }
}
