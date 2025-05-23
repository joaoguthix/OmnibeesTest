using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Context;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Infrastructure.Repository.TipoParentescoRepositories
{
    public class TipoParentescoRepository : ITipoParentescoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public TipoParentescoRepository(
                                  IDbConnectionFactory connectionFactory,
                                  IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }
        public async Task<IEnumerable<TipoParentesco>> GetTipoParentescosAsync()
        {
            var sql = "SELECT * FROM TipoParentesco";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryAsync<TipoParentesco>(sql);
        }

        public async Task<TipoParentesco?> GetTipoParentescoByIdAsync(int id)
        {
            var sql = @"SELECT * FROM TipoParentesco c WHERE c.Id = @Id";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QuerySingleOrDefaultAsync<TipoParentesco>(sql, new { Id = id });
        }
    }
}
