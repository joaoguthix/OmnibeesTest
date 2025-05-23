using Application.DTOs;
using Application.DTOs.Maps;
using Application.Interfaces;
using Domain.Aggregate;
using Domain.Interfaces.InterfaceBusiness;
using Infrastructure.Configuration.UnitOfWork;

namespace Application.Services
{
    public class CotacaoAppService : ICotacaoAppService
    {
        private readonly ICotacaoBusiness _cotacaoBusiness;
        private readonly ICotacaoCoberturaAppService _cotacaoCoberturaAppService;
        private readonly ICotacaoBeneficiarioAppService _cotacaBeneficiarioAppService;
        private readonly IUnitOfWork _uow;
        private readonly ILogBusiness _logBusiness;

        public CotacaoAppService(
            ICotacaoBusiness cotacaoBusiness,
            IUnitOfWork uow,
            ILogBusiness logBusiness,
            ICotacaoBeneficiarioAppService cotacaBeneficiarioAppService,
            ICotacaoCoberturaAppService cotacaoCoberturaAppService)
        {
            _cotacaoBusiness = cotacaoBusiness;
            _uow = uow;
            _logBusiness = logBusiness;
            _cotacaBeneficiarioAppService = cotacaBeneficiarioAppService;
            _cotacaoCoberturaAppService = cotacaoCoberturaAppService;
        }

        public async Task<int> AddCotacaoAsync(CotacaoRequestDTO cotacao)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var mapCotacao = CotacaoMap.MapToCotacaoEntity(cotacao);
                mapCotacao.Id = await _cotacaoBusiness.AddCotacaoAsync(mapCotacao);

                if (cotacao.CotacaoesCoberturas.Count() > 0)
                {
                    await _cotacaoCoberturaAppService.AddCotacaoCoberturaAsync(cotacao.CotacaoesCoberturas, mapCotacao);
                }

                if (cotacao.CotacaoBeneficiarios.Count() > 0)
                {
                    await _cotacaBeneficiarioAppService.AddCotacaoBeneficiarioAsync(cotacao.CotacaoBeneficiarios, mapCotacao.Id);
                }

                await _cotacaoBusiness.AtualizarPremioAsync(mapCotacao, cotacao.CotacaoesCoberturas);

                await _uow.CommitAsync();
                return mapCotacao.Id;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();

                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        public async Task AtualizarCotacaoAsync(CotacaoUpdateRequestDTO cotacaoDto)
        {
            await _uow.BeginTransactionAsync();

            try
            {
                var cotacao = CotacaoMap.MapToCotacao(cotacaoDto);

                var cotacaoExistente = await _cotacaoBusiness.GetCotacaoByIdAsync(cotacao.Id, cotacaoDto.Secret);

                await _cotacaoBusiness.AtualizarCotacaoAsync(MapUpdateObject.AplicarDiferencasCotacao(cotacaoExistente, cotacao));

                var getCotacaoCobertura = await _cotacaoCoberturaAppService.GetCotacaoCoberturaByIdCotacaoAsync(cotacao.Id, cotacaoDto.Secret);

                await _cotacaoBusiness.AtualizarPremioAsync(cotacao, CotacaoMap.MapToCotacaoCobertura(getCotacaoCobertura));

                await _uow.CommitAsync();
            }
            catch (Exception)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<CotacaoAggregate?> GetCotacaoDetailsByIdAsync(int idCotacao, string secret)
        {
            try
            {
                return await _cotacaoBusiness.GetCotacaoDetailsByIdAsync(idCotacao, secret);
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        public async Task<List<CotacaoProdutoDTO>> GetCotacoesByParceiroAsync(string secret, int pageNumber, int pageSize)
        {
            try
            {
                var cotacoes = await _cotacaoBusiness.GetCotacoesByParceiroAsync(secret, pageNumber, pageSize);
                return CotacaoMap.MapToCotacaoProduto(cotacoes);
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }
        public async Task ExcluirCotacaoAsync(int idCotacao, string secret)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                await _cotacaoBusiness.ExcluirCotacaoAsync(idCotacao, secret);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        //public async Task AtualizarCotacaoAsync(CotacaoRequestDTO cotacaoDto)
        //{
        //    await _uow.BeginTransactionAsync();
        //    try
        //    {
        //        var mapCotacao = CotacaoMap.MapToCotacaoEntity(cotacaoDto);

        //        await _cotacaoBusiness.AtualizarCotacaoAsync(mapCotacao);

        //        if (cotacaoDto.CotacaoesCoberturas.Any())
        //        {
        //            await _cotacaoCoberturaAppService.AtualizarCotacaoCoberturasAsync(cotacaoDto.CotacaoesCoberturas, mapCotacao);
        //        }

        //        if (cotacaoDto.CotacaoBeneficiarios.Any())
        //        {
        //            await _cotacaBeneficiarioAppService.AtualizarCotacaoBeneficiariosAsync(cotacaoDto.CotacaoBeneficiarios, mapCotacao.Id);
        //        }

        //        await _cotacaoBusiness.AtualizarPremioAsync(mapCotacao, cotacaoDto.CotacaoesCoberturas);

        //        await _uow.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await _uow.RollbackAsync();
        //        throw;
        //    }
        //}
    }
}
