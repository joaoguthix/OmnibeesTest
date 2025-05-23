using Domain.Entities;

namespace Domain.Interfaces.InterfaceRepository
{
    public interface IParceiroRepository
    {
        Task<Parceiro> GetParceiroBySecretAsync(string secret);
    }
}
