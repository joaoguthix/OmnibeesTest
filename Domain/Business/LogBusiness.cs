using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;

namespace Domain.Business
{
    public class LogBusiness : ILogBusiness
    {
        private readonly ILogRepository _logRepository;

        public LogBusiness(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task RegistrarLogAsync(string description)
        {
            await _logRepository.RegistrarLogAsync(description);
        }

    }
}
