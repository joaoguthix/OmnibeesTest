namespace Domain.Entities
{
    public class CotacaoCobertura
    {
        public int Id { get; set; } // PK
        public int IdCotacao { get; set; } // Cotação (FK)
        public int IdCobertura { get; set; } // Cobertura (FK)
        public decimal? ValorDesconto { get; set; } // 2 casas decimais
        public decimal? ValorAgravo { get; set; } // 2 casas decimais
        public decimal? ValorTotal { get; set; }
    }
}
