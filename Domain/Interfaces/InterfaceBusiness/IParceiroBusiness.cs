using Domain.Entities;

namespace Domain.Interfaces.InterfaceBusiness
{
    public interface IParceiroBusiness
    {
        Task<Parceiro> GetParceiroBySecretAsync(string secret);
    }
}
