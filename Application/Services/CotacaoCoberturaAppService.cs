using Application.DTOs;
using Application.DTOs.Maps;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Application.Services
{
    public class CotacaoCoberturaAppService : ICotacaoCoberturaAppService
    {
        private readonly ICotacaoCoberturaBusiness _cotacaoCoberturaBusiness;
        private readonly IUnitOfWork _uow;
        private readonly ICotacaoBusiness _cotacaoBusiness;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogBusiness _logBusiness;

        public CotacaoCoberturaAppService(
            ICotacaoCoberturaBusiness cotacaoCoberturaBusiness,
            IUnitOfWork uow,
            ICotacaoBusiness cotacaoBusiness,
            IDbConnectionFactory connectionFactory,
            ILogBusiness logBusiness)
        {
            _cotacaoCoberturaBusiness = cotacaoCoberturaBusiness;
            _uow = uow;
            _cotacaoBusiness = cotacaoBusiness;
            _connectionFactory = connectionFactory;
            _logBusiness = logBusiness;
        }

        public async Task<List<int>> AddCotacaoCoberturaAsync(List<CotacaoCobertura> cotacaoCobertura, Cotacao cotacao)
        {
            try
            {
                return await _cotacaoCoberturaBusiness.AddCotacaoCoberturaAsync(cotacaoCobertura, cotacao);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddNovaCotacaoCoberturaAsync(List<CotacaoCobertura> cotacaoCobertura, int idCotacao, string secret)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                var getCotacao = await _cotacaoBusiness.GetCotacaoByIdAsync(idCotacao, secret);

                await _cotacaoCoberturaBusiness.AddCotacaoCoberturaAsync(cotacaoCobertura, getCotacao);

                await _cotacaoBusiness.AtualizarPremioAsync(getCotacao, cotacaoCobertura);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        public async Task RemoverCotacaoCoberturaAsync(int idCotacao, string secret, int IdCotacaoCobertura)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                await _cotacaoCoberturaBusiness.RemoverCotacaoCoberturaAsync(idCotacao, secret, IdCotacaoCobertura);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        public async Task<List<CotacaoCoberturaDTO>> GetCotacaoCoberturaByIdCotacaoAsync(int idCotacao, string secret)
        {
            try
            {
                return CotacaoMap.MapToCotacaoCobertura(await _cotacaoCoberturaBusiness.GetCotacaoCoberturaByIdCotacaoAsync(idCotacao, secret));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
