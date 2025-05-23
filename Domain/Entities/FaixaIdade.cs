namespace Domain.Entities
{
    public class FaixaIdade
    {
        public int Id { get; set; } // PK
        public string? Description { get; set; }
        public string Desconto { get; set; }
        public string Agravo { get; set; }
    }

    public class FaixaIdadeDto
    {
        public int IdadeMinima { get; set; }
        public int IdadeMaxima { get; set; }
        public string Desconto { get; set; }
        public string Agravo { get; set; }
    }
}