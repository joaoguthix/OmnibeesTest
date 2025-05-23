using Domain.Entities;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface ICoberturaRepository
    {
        Task<IEnumerable<Cobertura>> GetCoberturasAsync();
    }
}
