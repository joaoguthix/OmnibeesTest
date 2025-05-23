using Domain.Entities;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface IFaixaIdadeRepository
    {
        Task<IEnumerable<FaixaIdade>> GetFaixaIdadesAsync();
    }
}
