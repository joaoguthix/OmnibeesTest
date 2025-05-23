using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProdutoAppService
    {
        //Task<IEnumerable<Produto>> ObterTodosAsync();
        //Task<Produto?> ObterPorIdAsync(int id);
        Task CriarAsync(Produto produto);
    }
}
