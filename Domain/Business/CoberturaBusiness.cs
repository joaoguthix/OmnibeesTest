using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;

namespace Domain.Business
{
    public class CoberturaBusiness : ICoberturaBusiness
    {
        private readonly ICoberturaRepository _coberturaRepository;
        public CoberturaBusiness(ICoberturaRepository coberturaRepository)
        {
            _coberturaRepository = coberturaRepository;
        }

        public async Task<IEnumerable<Cobertura>> GetCoberturasAsync()
        {
            try
            {
                return await _coberturaRepository.GetCoberturasAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
