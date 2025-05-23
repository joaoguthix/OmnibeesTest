using Domain.Entities;

namespace Application.DTOs
{
    public class CotacaoRequestDTO
    {
        public int Id { get; set; } // PK

        public int IdProduto { get; set; } // Produto cotado (FK)
        public int IdParceiro { get; set; } // Parceiro (FK)

        public string? NomeSegurado { get; set; } // Limite 100
        public int? DDD { get; set; }
        public long? Telefone { get; set; }

        public string? Endereco { get; set; } // Limite 255
        public string? CEP { get; set; }
        public string? Documento { get; set; }

        public DateTime? Nascimento { get; set; }

        public decimal? Premio { get; set; } // 2 casas decimais
        public decimal ImportanciaSegurada { get; set; } // 2 casas decimais
        public string Secret { get; set; }
        public List<CotacaoCobertura> CotacaoesCoberturas { get; set; }

        public List<CotacaoBeneficiario?> CotacaoBeneficiarios { get; set; }
    }
}
