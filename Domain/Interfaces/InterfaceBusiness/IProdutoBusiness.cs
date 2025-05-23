using Domain.Entities;

namespace Domain.Interfaces.InterfaceBusiness
{
    public interface IProdutoBusiness
    {
        //Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(int id);
        Task CriarAsync(Produto produto);
    }
}
