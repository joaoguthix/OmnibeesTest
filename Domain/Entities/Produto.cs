namespace Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal BaseValue { get; set; }
        public decimal Limit { get; set; }
    }
}
