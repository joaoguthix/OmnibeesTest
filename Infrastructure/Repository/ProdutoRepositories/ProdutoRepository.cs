using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Context;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.ProdutoRepositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public ProdutoRepository(IDbConnectionFactory connectionFactory,
                                 IUnitOfWork uow)
        {
            //_readContext = readContext;
            //_writeContext = writeContext;
            _connectionFactory = connectionFactory;
            _uow = uow;
        }

        //public async Task<IEnumerable<Produto>> ObterTodosViaEFAsync()
        //{
        //    return await _readContext.Produto.AsNoTracking().ToListAsync();
        //}

        public async Task<IEnumerable<Produto>> ObterTodosViaDapperAsync()
        {
            var sql = "SELECT Id, Description, BaseValue, Limit FROM Produtos";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryAsync<Produto>(sql);
        }

        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            var sql = "SELECT Id, Description, BaseValue, Limit FROM Produto WHERE Id = @Id";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task CriarAsync(Produto produto)
        {
            try
            {
                await _uow.DbContext.Set<Produto>().AddAsync(produto);
            }
            catch
            {
                throw;
            }
        }
    }
}
