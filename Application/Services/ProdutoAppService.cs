using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoBusiness _produtoBusiness;
        private readonly IUnitOfWork _uow;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogBusiness _logBusiness;
        public ProdutoAppService(
            IProdutoBusiness produtoBusiness,
            IUnitOfWork uow,
            IDbConnectionFactory connectionFactory,
            ILogBusiness logBusiness)
        {
            _produtoBusiness = produtoBusiness;
            _uow = uow;
            _connectionFactory = connectionFactory;
            _logBusiness = logBusiness;
        }

        //public async Task<IEnumerable<Produto>> ObterTodosAsync()
        //{
        //    return await _produtoBusiness.ObterTodosAsync();
        //}

        //public async Task<Produto?> ObterPorIdAsync(int id)
        //{
        //    return await _produtoBusiness.ObterPorIdAsync(id);
        //}

        public async Task CriarAsync(Produto produto)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                await _produtoBusiness.CriarAsync(produto);
                await _logBusiness.RegistrarLogAsync($"Produto criado: {produto.Description}");

                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}