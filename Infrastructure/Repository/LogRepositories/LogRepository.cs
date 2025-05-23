using Dapper;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;
using System.Data;

namespace Infrastructure.Repository.LogRepositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IDbConnection _connection;
        private readonly IUnitOfWork _uow;

        public LogRepository(IDbConnectionFactory factory, IUnitOfWork uow)
        {
            _connection = factory.GetWriteConnection();
            _uow = uow;
        }

        public async Task RegistrarLogAsync(string descricao)
        {
            var sql = "INSERT INTO LogOperacao (Descricao) VALUES (@desc)";
            await _uow.Connection.ExecuteAsync(sql, new { desc = descricao }, _uow.Transaction);
        }
    }
}
