namespace Domain.Interfaces.InterfaceRepository
{
    public interface ILogRepository
    {
        Task RegistrarLogAsync(string descricao);
    }
}
