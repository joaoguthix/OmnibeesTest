using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Context;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Infrastructure.Repository.FaixaIdadeRepository
{
    public class FaixaIdadeRepository : IFaixaIdadeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public FaixaIdadeRepository(
                                  IDbConnectionFactory connectionFactory,
                                  IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }

        public async Task<IEnumerable<FaixaIdade>> GetFaixaIdadesAsync()
        {
            var sql = "SELECT * FROM FaixaIdade";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryAsync<FaixaIdade>(sql);
        }

        public async Task<FaixaIdade?> GetFaixaIdadeByIdAsync(int id)
        {
            var sql = @"SELECT * FROM FaixaIdade c WHERE c.Id = @Id";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QuerySingleOrDefaultAsync<FaixaIdade>(sql, new { Id = id });
        }
    }
}
