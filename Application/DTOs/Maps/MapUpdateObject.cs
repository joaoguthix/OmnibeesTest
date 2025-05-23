using Domain.Entities;

namespace Application.DTOs.Maps
{
    public static class MapUpdateObject
    {
        public static Cotacao AplicarDiferencasCotacao(Cotacao cotacaoExistente, Cotacao cotacaoAtualizada)
        {
            if (!string.IsNullOrWhiteSpace(cotacaoAtualizada.NomeSegurado) && cotacaoExistente.NomeSegurado != cotacaoAtualizada.NomeSegurado)
                cotacaoExistente.NomeSegurado = cotacaoAtualizada.NomeSegurado;

            if (cotacaoAtualizada.DDD.HasValue && cotacaoExistente.DDD != cotacaoAtualizada.DDD)
                cotacaoExistente.DDD = cotacaoAtualizada.DDD;

            if (cotacaoAtualizada.Telefone.HasValue && cotacaoExistente.Telefone != cotacaoAtualizada.Telefone)
                cotacaoExistente.Telefone = cotacaoAtualizada.Telefone;

            if (!string.IsNullOrWhiteSpace(cotacaoAtualizada.Endereco) && cotacaoExistente.Endereco != cotacaoAtualizada.Endereco)
                cotacaoExistente.Endereco = cotacaoAtualizada.Endereco;

            if (!string.IsNullOrWhiteSpace(cotacaoAtualizada.CEP) && cotacaoExistente.CEP != cotacaoAtualizada.CEP)
                cotacaoExistente.CEP = cotacaoAtualizada.CEP;

            if (!string.IsNullOrWhiteSpace(cotacaoAtualizada.Documento) && cotacaoExistente.Documento != cotacaoAtualizada.Documento)
                cotacaoExistente.Documento = cotacaoAtualizada.Documento;

            if (cotacaoAtualizada.ImportanciaSegurada != cotacaoExistente.ImportanciaSegurada)
                cotacaoExistente.ImportanciaSegurada = cotacaoAtualizada.ImportanciaSegurada;

            if (cotacaoAtualizada.IdProduto != cotacaoExistente.IdProduto)
                cotacaoExistente.IdProduto = cotacaoAtualizada.IdProduto;

            cotacaoExistente.Nascimento = cotacaoAtualizada.Nascimento;
            cotacaoExistente.DataAtualizacao = DateTime.Now;

            return cotacaoExistente;
        }
    }
}
