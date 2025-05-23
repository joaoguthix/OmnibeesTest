using Domain.Entities;

namespace Application.DTOs.Maps
{
    public class CotacaoAggregateDTO : CotacaoDTO
    {
        public List<CotacaoBeneficiario> CotacaoBeneficiarios { get; set; }
        public List<CotacaoCobertura> CotacaoCoberturas { get; set; }
    }
}
