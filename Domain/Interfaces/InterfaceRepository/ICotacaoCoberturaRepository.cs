using Domain.Entities;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface ICotacaoCoberturaRepository
    {
        Task<int> AddCotacaoCoberturaAsync(CotacaoCobertura cotacaoCobertura);
        Task<List<CotacaoCobertura>> GetCotacaoCoberturaByIdCotacaoAsync(int idCotacao, int idParceiro);
        Task<bool> RemoveCotacaoCoberturaAsync(int idCotacaoCobertura);
    }
}
