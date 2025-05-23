namespace Domain.Entities
{
    public class CotacaoBeneficiario
    {
        public int Id { get; set; } // PK
        public int IdCotacao { get; set; } // Cotação (FK)
        public int IdParentesco { get; set; } // Parentesco (FK)
        public string? Nome { get; set; } // Limite 100
        public decimal? Percentual { get; set; } // 2 casas decimais
    }
}
