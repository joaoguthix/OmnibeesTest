using Domain.Entities;
using Domain.ViewModel;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface ICotacaoBeneficiarioRepository
    {
        Task<int> AddCotacaoBeneficiarioAsync(CotacaoBeneficiario cotacaoBeneficiario);
        Task<List<CotacaoBeneficiarioDetailViewModel>> DetalharBeneficiarioAsync(int idCotacao, int idParceiro);
        Task<List<CotacaoBeneficiarioViewModel>> ListarBeneficiariosPorCotacaoAsync(int idCotacao, int idParceiro);
        Task<List<CotacaoBeneficiario>> GetCotacaoBeneficiarioByIdCotacaoAsync(int idCotacao, int idParceiro);
        Task RemoverCotacaoBeneficiarioAsync(int idBeneficiario);

    }
}
