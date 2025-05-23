using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Context;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Infrastructure.Repository.ParceiroRepository
{
    public class ParceiroRepository : IParceiroRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public ParceiroRepository(
                                  IDbConnectionFactory connectionFactory,
                                  IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }
        public async Task<IEnumerable<Parceiro>> GetParceirosAsync()
        {
            var sql = "SELECT * FROM Parceiro";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryAsync<Parceiro>(sql);
        }

        public async Task<Parceiro?> GetParceiroBySecretAsync(string secret)
        {
            var sql = @"SELECT * FROM Parceiro c WHERE c.Secret = @Secret";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QuerySingleOrDefaultAsync<Parceiro?>(sql, new { Secret = secret });
        }
    }
}
