namespace Application.DTOs
{
    public class CotacaoCoberturaDTO
    {
        public int Id { get; set; }
        public int IdCobertura { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? ValorAgravo { get; set; }
        public decimal? ValorTotal { get; set; }
    }
}
