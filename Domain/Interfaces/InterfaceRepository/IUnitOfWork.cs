using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Configuration.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();
    }
}
