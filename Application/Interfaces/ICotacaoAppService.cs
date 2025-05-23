using Application.DTOs;
using Domain.Aggregate;

namespace Application.Interfaces
{
    public interface ICotacaoAppService
    {
        Task<int> AddCotacaoAsync(CotacaoRequestDTO cotacao);
        Task AtualizarCotacaoAsync(CotacaoUpdateRequestDTO cotacaoDto);
        Task<CotacaoAggregate?> GetCotacaoDetailsByIdAsync(int idCotacao, string secret);
        Task<List<CotacaoProdutoDTO>> GetCotacoesByParceiroAsync(string secret, int pageNumber, int pageSize);
        Task ExcluirCotacaoAsync(int idCotacao, string secret);
    }
}
