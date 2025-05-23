using Domain.Aggregate;
using Domain.Entities;

namespace Application.DTOs.Maps
{
    public static class CotacaoMap
    {
        public static T MapToEntity<T>(Dictionary<string, object> input) where T : class, new()
        {
            T entity = new T();
            foreach (var property in input)
            {
                var prop = entity.GetType().GetProperty(property.Key);
                if (prop != null)
                {
                    prop.SetValue(entity, property.Value);
                }
            }
            return entity;
        }

        public static Cotacao MapToCotacaoEntity(CotacaoRequestDTO dto)
        {
            return new Cotacao
            {
                Id = dto.Id,
                NomeSegurado = dto.NomeSegurado,
                DDD = dto.DDD,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                CEP = dto.CEP,
                Documento = dto.Documento,
                Nascimento = dto.Nascimento,
                IdProduto = dto.IdProduto,
                IdParceiro = dto.IdParceiro,
                Premio = dto.Premio,
                ImportanciaSegurada = dto.ImportanciaSegurada,

            };
        }

        public static Cotacao MapToCotacao(CotacaoUpdateRequestDTO dto)
        {
            return new Cotacao
            {
                Id = dto.Id,
                NomeSegurado = dto.NomeSegurado,
                DDD = dto.DDD,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                CEP = dto.CEP,
                Documento = dto.Documento,
                Nascimento = dto.Nascimento,
                IdProduto = dto.IdProduto,
                IdParceiro = dto.IdParceiro,
                Premio = dto.Premio,
                ImportanciaSegurada = dto.ImportanciaSegurada,

            };
        }

        public static List<CotacaoProdutoDTO> MapToCotacaoProduto(List<CotacaoProdutoAggregate> cotacoes)
        {
            return cotacoes.Select(cotacao => new CotacaoProdutoDTO
            {
                Id = cotacao.Id,
                NomeSegurado = cotacao.NomeSegurado,
                Documento = cotacao.Documento,
                Description = cotacao.Produto.Description
            }).ToList();
        }
        public static List<CotacaoCoberturaDTO> MapToCotacaoCobertura(List<CotacaoCobertura> cotacoes)
        {
            return cotacoes.Select(cotacao => new CotacaoCoberturaDTO
            {
                Id = cotacao.Id,
                ValorAgravo = cotacao.ValorAgravo,
                ValorDesconto = cotacao.ValorDesconto,
                ValorTotal = cotacao.ValorTotal,
                IdCobertura = cotacao.IdCobertura
            }).ToList();
        }
        public static List<CotacaoCobertura> MapToCotacaoCobertura(List<CotacaoCoberturaDTO> cotacoes)
        {
            return cotacoes.Select(cotacao => new CotacaoCobertura
            {
                Id = cotacao.Id,
                ValorAgravo = cotacao.ValorAgravo,
                ValorDesconto = cotacao.ValorDesconto,
                ValorTotal = cotacao.ValorTotal,
                IdCobertura = cotacao.IdCobertura
            }).ToList();
        }

        //public List<CotacaoBeneficiario> MapToCotacaoBeneficiarioEntities(List<CotacaoBeneficiarioDTO> dtos, int idCotacao)
        //{
        //    return dtos.Select(dto => new CotacaoBeneficiario
        //    {
        //        Id = dto.Id,
        //        Nome = dto.Nome,
        //        Percentual = dto.Percentual,
        //        IdCotacao = idCotacao
        //    }).ToList();
        //}

        //public List<CotacaoCobertura> MapToCotacaoCoberturaEntities(List<CotacaoCobertura> dtos, int idCotacao)
        //{
        //    return dtos.Select(dto => new CotacaoCobertura
        //    {
        //        Id = dto.Id,
        //        ValorAgravo = dto.ValorAgravo,
        //        ValorDesconto = dto.ValorDesconto,
        //        IdCotacao = idCotacao,
        //        ValorTotal = dto.ValorTotal
        //    }).ToList();
        //}
    }
}
