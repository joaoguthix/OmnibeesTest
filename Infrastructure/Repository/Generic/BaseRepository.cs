using System.Data;
using Dapper;
using Infrastructure.Configuration.Dapper;

namespace Infrastructure.Repository.Generic
{
    public class BaseRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BaseRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        //public IQueryable<T> ReadFromDbContext<T>() where T : class =>
        //    _readContext.Set<T>();

        //public void WriteToDbContext<T>(T entity) where T : class
        //{
        //    _writeContext.Set<T>().Add(entity);
        //    _writeContext.SaveChanges();
        //}

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            using var connection = _connectionFactory.GetReadConnection();
            return connection.Query<T>(sql, parameters);
        }

        public void ExecuteDapper(string sql, object parameters)
        {
            using var connection = _connectionFactory.GetWriteConnection();
            connection.Execute(sql, parameters);
        }

        public void ExecuteDapperWithTransaction(Action<IDbTransaction> action)
        {
            using var connection = _connectionFactory.GetWriteConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                action(transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}