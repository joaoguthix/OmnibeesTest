using Domain.Entities;

namespace Domain.Interfaces.InterfaceBusiness
{
    public interface ICoberturaBusiness
    {
        Task<IEnumerable<Cobertura>> GetCoberturasAsync();
    }
}
