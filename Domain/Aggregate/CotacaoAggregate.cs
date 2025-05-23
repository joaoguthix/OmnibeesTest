using Domain.Entities;

namespace Domain.Aggregate
{
    public class CotacaoAggregate : Cotacao
    {
        public List<CotacaoBeneficiario> CotacaoBeneficiarios { get; set; }
        public List<CotacaoCobertura> CotacaoCoberturas { get; set; }
    }
}
