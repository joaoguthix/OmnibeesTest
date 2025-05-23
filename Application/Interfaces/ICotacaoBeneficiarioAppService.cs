using Domain.Entities;
using Domain.ViewModel;

namespace Application.Interfaces
{
    public interface ICotacaoBeneficiarioAppService
    {
        Task AddCotacaoBeneficiarioAsync(List<CotacaoBeneficiario?> cotacaoBeneficiario, int idCotacao);
        Task AtualizarCotacaoBeneficiariosAsync(int idCotacao, string secret, List<CotacaoBeneficiario> novosBeneficiarios);
        Task RemoverCotacaoBeneficiarioAsync(int idCotacao, string secret, int removeBeneficiarioId);
        Task<List<CotacaoBeneficiarioDetailViewModel>> DetalharBeneficiarioAsync(int idCotacao, string secret);
        Task<List<CotacaoBeneficiarioViewModel>> ListarBeneficiariosPorCotacaoAsync(int idCotacao, string secret);
    }
}
