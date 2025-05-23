using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.ViewModel;
using Infrastructure.Configuration.UnitOfWork;

namespace Application.Services
{
    public class CotacaoBeneficiarioAppService : ICotacaoBeneficiarioAppService
    {
        private readonly ICotacaoBeneficiarioBusiness _cotacaoBeneficiarioBusiness;
        private readonly IUnitOfWork _uow;
        private readonly ILogBusiness _logBusiness;

        public CotacaoBeneficiarioAppService(
            ICotacaoBeneficiarioBusiness cotacaoBeneficiarioBusiness,
            IUnitOfWork uow,
            ILogBusiness logBusiness)
        {
            _cotacaoBeneficiarioBusiness = cotacaoBeneficiarioBusiness;
            _uow = uow;
            _logBusiness = logBusiness;
        }

        public async Task AddCotacaoBeneficiarioAsync(List<CotacaoBeneficiario?> cotacaoBeneficiario, int idCotacao)
        {
            try
            {
                await _cotacaoBeneficiarioBusiness.AddCotacaoBeneficiarioAsync(cotacaoBeneficiario, idCotacao);
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        public async Task AtualizarCotacaoBeneficiariosAsync(int idCotacao, string secret, List<CotacaoBeneficiario> novosBeneficiarios)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                await _cotacaoBeneficiarioBusiness.AtualizarCotacaoBeneficiariosAsync(idCotacao, secret, novosBeneficiarios);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                await _uow.RollbackAsync();
                throw;
            }
        }
        public async Task RemoverCotacaoBeneficiarioAsync(int idCotacao, string secret, int removeBeneficiarioId)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                await _cotacaoBeneficiarioBusiness.RemoverCotacaoBeneficiarioAsync(idCotacao, secret, removeBeneficiarioId);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<List<CotacaoBeneficiarioDetailViewModel>> DetalharBeneficiarioAsync(int idCotacao, string secret)
        {
            try
            {
                var result = await _cotacaoBeneficiarioBusiness.DetalharBeneficiarioAsync(idCotacao, secret);
                return result;
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }

        public async Task<List<CotacaoBeneficiarioViewModel>> ListarBeneficiariosPorCotacaoAsync(int idCotacao, string secret)
        {
            try
            {
                var result = await _cotacaoBeneficiarioBusiness.ListarBeneficiariosPorCotacaoAsync(idCotacao, secret);
                return result;
            }
            catch (Exception ex)
            {
                //await _logBusiness.LogErrorAsync(ex);
                throw;
            }
        }
    }
}
