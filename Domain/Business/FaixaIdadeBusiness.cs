using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;

namespace Domain.Business
{
    public class FaixaIdadeBusiness : IFaixaIdadeBusiness
    {
        private readonly IFaixaIdadeRepository _faixaIdadeRepository;

        public FaixaIdadeBusiness(IFaixaIdadeRepository faixaIdadeRepository)
        {
            _faixaIdadeRepository = faixaIdadeRepository;

        }

        public async Task<IEnumerable<FaixaIdadeDto>> GetFaixaIdadesAsync()
        {
            var faixasOriginais = await _faixaIdadeRepository.GetFaixaIdadesAsync();

            var faixas = faixasOriginais
                .Select(f =>
                {
                    var partes = System.Text.RegularExpressions.Regex
                        .Match(f.Description, @"(\d+)\s*a\s*(\d+)")
                        .Groups;

                    return new FaixaIdadeDto
                    {
                        IdadeMinima = int.Parse(partes[1].Value),
                        IdadeMaxima = int.Parse(partes[2].Value),
                        Desconto = f.Desconto,
                        Agravo = f.Agravo
                    };
                })
                .ToList();
            return faixas;
        }
    }
}
