using Domain.Entities;

namespace Domain.Interfaces.InterfaceBusiness
{
    public interface IFaixaIdadeBusiness
    {
        Task<IEnumerable<FaixaIdadeDto>> GetFaixaIdadesAsync();
    }
}
