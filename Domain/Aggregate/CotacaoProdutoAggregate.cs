using Domain.Entities;

namespace Domain.Aggregate
{
    public class CotacaoProdutoAggregate : Cotacao
    {
        public Produto Produto { get; set; }
    }
}
