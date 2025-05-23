using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Infrastructure.Repository.CoberturaRepositories
{
    public class CoberturaRepository : ICoberturaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public CoberturaRepository(
                                  IDbConnectionFactory connectionFactory,
                                  IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }

        public async Task<IEnumerable<Cobertura>> GetCoberturasAsync()
        {
            var sql = "SELECT * FROM Cobertura";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryAsync<Cobertura>(sql);
        }

        public async Task<Cobertura?> GetCoberturaByIdAsync(int id)
        {
            var sql = @"SELECT * FROM Cotacao c WHERE c.Id = @Id";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QuerySingleOrDefaultAsync<Cobertura>(sql, new { Id = id });
        }

        //public async Task<List<Cobertura>> GetCoberturasViaEFAsync()
        //{
        //    return await _readContext.Cobertura.AsNoTracking().ToListAsync();
        //}
    }
}
