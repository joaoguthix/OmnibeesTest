using Infrastructure.Configuration.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Configuration.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly WriteContextSqlServer _context;
        private DbTransaction _transaction;

        public UnitOfWork(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("WriteConnection"));
            _context = new WriteContextSqlServer(_connection); // mesma conexão para EF e Dapper
        }

        public DbContext DbContext => _context;
        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public async Task BeginTransactionAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            _transaction = await _connection.BeginTransactionAsync();
            _context.Database.UseTransaction(_transaction);// compartilhar transação com o EF e Dapper, facilita gerenciamento e atomicidade
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _connection.CloseAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _connection.CloseAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
