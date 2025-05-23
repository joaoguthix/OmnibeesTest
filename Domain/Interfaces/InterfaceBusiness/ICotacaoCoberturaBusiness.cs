using Domain.Entities;

namespace Domain.Interfaces.InterfaceBusiness
{
    public interface ICotacaoCoberturaBusiness
    {
        Task<List<int>> AddCotacaoCoberturaAsync(List<CotacaoCobertura> cotacaoCobertura, Cotacao cotacaobool,bool novaCotacao = false);
        Task RemoverCotacaoCoberturaAsync(int idCotacao, string secret, int removeCoberturaId);
        Task<List<CotacaoCobertura>> GetCotacaoCoberturaByIdCotacaoAsync(int idCotacao, string secret);
    }
}
