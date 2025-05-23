using Domain.Aggregate;
using Domain.Entities;

namespace Domain.Interfaces.InterfaceBusiness
{
    public interface ICotacaoBusiness
    {
        Task<int> AddCotacaoAsync(Cotacao cotacao);
        Task AtualizarCotacaoAsync(Cotacao cotacao);
        Task AtualizarPremioAsync(Cotacao cotacao, List<CotacaoCobertura> cotacaoCoberturas);
        Task<CotacaoAggregate?> GetCotacaoDetailsByIdAsync(int idCotacao, string secret);
        Task<List<CotacaoProdutoAggregate>> GetCotacoesByParceiroAsync(string secret, int pageNumber, int pageSize);
        Task<Cotacao> GetCotacaoByIdAsync(int id, string secret);
        Task ExcluirCotacaoAsync(int idCotacao, string secret);
        Task<decimal?> CalculateValuePremio(Cotacao cotacao, List<CotacaoCobertura> cotacaoCoberturas);
    }
}
