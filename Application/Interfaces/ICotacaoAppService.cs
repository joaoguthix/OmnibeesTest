using Application.DTOs;
using Application.DTOs.Maps;
using Domain.Aggregate;

namespace Application.Interfaces
{
    public interface ICotacaoAppService
    {
        Task<int> AddCotacaoAsync(CotacaoRequestDTO cotacao);
        Task AtualizarCotacaoAsync(CotacaoUpdateRequestDTO cotacaoDto);
        Task<CotacaoAggregateDTO> GetCotacaoDetailsByIdAsync(int idCotacao, string secret);
        Task<List<CotacaoProdutoDTO>> GetCotacoesByParceiroAsync(string secret, int pageNumber, int pageSize);
        Task ExcluirCotacaoAsync(int idCotacao, string secret);
    }
}
