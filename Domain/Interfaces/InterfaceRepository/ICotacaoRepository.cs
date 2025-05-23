using Domain.Aggregate;
using Domain.Entities;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface ICotacaoRepository
    {
        Task<int> AddCotacaoAsync(Cotacao cotacao);
        Task AtualizarCotacaoAsync(Cotacao cotacao);
        Task AtualizarImportanciaPremioAsync(int idCotacao, decimal? premio);
        Task<CotacaoAggregate?> GetCotacaoDetailsByIdAsync(int idCotacao, int idParceiro);
        Task<List<CotacaoProdutoAggregate>> GetCotacoesByParceiroAsync(int idParceiro, int pageNumber, int pageSize);
        Task<Cotacao?> GetCotacaoByIdAsync(int id, int idParceiro);
        Task ExcluirCotacaoAsync(int idCotacao, int idParceiro);
    }
}
