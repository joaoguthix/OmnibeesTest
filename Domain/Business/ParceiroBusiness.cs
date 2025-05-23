using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Domain.Business
{
    public class ParceiroBusiness : IParceiroBusiness
    {
        private readonly IParceiroRepository _parceiroRepository;
        private readonly IUnitOfWork _uow;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogBusiness _logBusiness;

        public ParceiroBusiness(IParceiroRepository parceiroRepository, IUnitOfWork unitOfWork, IDbConnectionFactory dbConnectionFactory, ILogBusiness logBusiness)
        {
            _parceiroRepository = parceiroRepository;
            _uow = unitOfWork;
            _connectionFactory = dbConnectionFactory;
            _logBusiness = logBusiness;
        }

        public async Task<Parceiro> GetParceiroBySecretAsync(string secret)
        {
            try
            {
                var response = await _parceiroRepository.GetParceiroBySecretAsync(secret);
                if (response == null)
                {
                    throw new ArgumentException("Parceiro não encontrado.");
                }
                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
