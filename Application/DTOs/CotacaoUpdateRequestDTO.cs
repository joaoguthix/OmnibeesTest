namespace Application.DTOs
{
    public class CotacaoUpdateRequestDTO
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public int IdParceiro { get; set; }
        public string NomeSegurado { get; set; }
        public int? DDD { get; set; }
        public long? Telefone { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Documento { get; set; }
        public DateTime Nascimento { get; set; }
        public decimal Premio { get; set; }
        public decimal ImportanciaSegurada { get; set; }
        public string Secret { get; set; }
    }
}
