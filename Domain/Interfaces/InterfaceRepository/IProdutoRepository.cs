using Domain.Entities;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface IProdutoRepository
    {
        //Task<IEnumerable<Produto>> ObterTodosViaEFAsync();
        Task<IEnumerable<Produto>> ObterTodosViaDapperAsync();
        Task<Produto?> ObterPorIdAsync(int id);
        Task CriarAsync(Produto produto);
    }
}
