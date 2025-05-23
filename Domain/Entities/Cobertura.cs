namespace Domain.Entities
{
    public class Cobertura
    {
        public int Id { get; set; } // PK
        public string Type { get; set; } // Limite 100
        public string? Description { get; set; } // Limite 255
        public decimal? Value { get; set; } // 2 casas decimais
    }
}
