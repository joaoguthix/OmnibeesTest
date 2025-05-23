using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICotacaoCoberturaAppService
    {
        Task<List<int>> AddCotacaoCoberturaAsync(List<CotacaoCobertura> cotacaoCobertura, Cotacao cotacao);
        Task AddNovaCotacaoCoberturaAsync(List<CotacaoCobertura> cotacaoCobertura, int idCotacao, string secret);
        Task RemoverCotacaoCoberturaAsync(int idCotacao, string secret, int IdCotacaoCobertura);

        Task<List<CotacaoCoberturaDTO>> GetCotacaoCoberturaByIdCotacaoAsync(int idCotacao, string secret);
    }
}
