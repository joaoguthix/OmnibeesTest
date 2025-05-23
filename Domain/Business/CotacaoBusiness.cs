using Domain.Aggregate;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;

namespace Domain.Business
{
    public class CotacaoBusiness : ICotacaoBusiness
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly IFaixaIdadeBusiness _faixaIdadeBusiness;
        private readonly IProdutoBusiness _produtoBusiness;
        private readonly IParceiroBusiness _parceiroBusiness;
        public CotacaoBusiness(ICotacaoRepository cotacaoRepository, IFaixaIdadeBusiness faixaIdadeBusiness, IProdutoBusiness produtoBusiness, IParceiroBusiness parceiroBusiness)
        {
            _cotacaoRepository = cotacaoRepository;
            _faixaIdadeBusiness = faixaIdadeBusiness;
            _produtoBusiness = produtoBusiness;
            _parceiroBusiness = parceiroBusiness;
        }

        public async Task<int> AddCotacaoAsync(Cotacao cotacao)
        {
            try
            {
                await ValidateCotacaoAsync(cotacao);
                await ValidateImportanciaSegurada(cotacao);

                cotacao.DataCriacao = DateTime.Now;
                cotacao.DataAtualizacao = DateTime.Now;
                return await _cotacaoRepository.AddCotacaoAsync(cotacao);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task AtualizarCotacaoAsync(Cotacao cotacao)
        {
            try
            {
                await ValidateCotacaoAsync(cotacao);
                await ValidateImportanciaSegurada(cotacao);
                cotacao.DataAtualizacao = DateTime.Now;
                await _cotacaoRepository.AtualizarCotacaoAsync(cotacao);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task AtualizarPremioAsync(Cotacao cotacao, List<CotacaoCobertura> cotacaoCoberturas)
        {
            try
            {
                var premio = await CalculateValuePremio(cotacao, cotacaoCoberturas);
                await _cotacaoRepository.AtualizarImportanciaPremioAsync(cotacao.Id, premio);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<CotacaoAggregate?> GetCotacaoDetailsByIdAsync(int idCotacao, string secret)
        {
            try
            {
                var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
                if (parceiro == null)
                {
                    throw new ArgumentException("Parceiro não encontrado.");
                }

                return await _cotacaoRepository.GetCotacaoDetailsByIdAsync(idCotacao, parceiro.Id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<CotacaoProdutoAggregate>> GetCotacoesByParceiroAsync(string secret, int pageNumber, int pageSize)
        {
            try
            {
                var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
                return await _cotacaoRepository.GetCotacoesByParceiroAsync(parceiro.Id, pageNumber, pageSize);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<Cotacao?> GetCotacaoByIdAsync(int id, string secret)
        {
            try
            {
                var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);

                var cotacao = await _cotacaoRepository.GetCotacaoByIdAsync(id, parceiro.Id);
                if (cotacao == null)
                    throw new ArgumentException("Cotação não encontrada.");

                return cotacao;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<decimal?> CalculateValuePremio(Cotacao cotacao, List<CotacaoCobertura> cotacaoCoberturas)
        {
            var getProduct = await _produtoBusiness.ObterPorIdAsync(cotacao.IdProduto);
            if (getProduct == null)
            {
                throw new ArgumentException("Produto não encontrado.");
            }

            var totalCoberturas = cotacaoCoberturas?.Sum(c => c.ValorTotal) ?? 0m;

            var premio = getProduct.BaseValue + totalCoberturas;

            return premio;
        }

        public async Task ExcluirCotacaoAsync(int idCotacao, string secret)
        {
            try
            {
                var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
                await _cotacaoRepository.ExcluirCotacaoAsync(idCotacao, parceiro.Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ValidateImportanciaSegurada(Cotacao cotacao)
        {
            var getProduct = await _produtoBusiness.ObterPorIdAsync(cotacao.IdProduto);
            if (getProduct == null)
            {
                throw new ArgumentException("Produto não encontrado.");
            }
            if (cotacao.ImportanciaSegurada < getProduct.BaseValue || cotacao.ImportanciaSegurada > getProduct.Limit)
            {
                throw new ArgumentException($"Importância Segurada deve estar entre {getProduct.BaseValue} e {getProduct.Limit}.");
            }
        }

        private async Task ValidateCotacaoAsync(Cotacao cotacao)
        {
            if ((cotacao.DDD != null && cotacao.Telefone == null) || (cotacao.DDD == null && cotacao.Telefone != null))
            {
                throw new ArgumentException("Se o DDD for informado, o Telefone também deve ser informado, e vice-versa.");
            }

            if (cotacao.DDD != null && cotacao.Telefone != null)
            {
                if (cotacao.DDD?.ToString().Length != 2 || cotacao.Telefone?.ToString().Length < 8 || cotacao.Telefone?.ToString().Length > 9)
                {
                    throw new ArgumentException("DDD deve ter 2 dígitos e Telefone deve ter entre 8 e 9 dígitos.");
                }
            }

            if (cotacao.NomeSegurado == null || cotacao.Nascimento == null)
            {
                throw new ArgumentException("Nome e Data de Nascimento são obrigatórios.");
            }

            if (cotacao.Nascimento != null)
            {
                var faixaIdades = await _faixaIdadeBusiness.GetFaixaIdadesAsync();
                var idade = DateTime.Now.Year - cotacao.Nascimento.Value.Year;
                if (cotacao.Nascimento > DateTime.Now.AddYears(-idade)) idade--;

                if (!faixaIdades.Any(faixa => idade >= faixa.IdadeMinima && idade <= faixa.IdadeMaxima))
                {
                    throw new ArgumentException("Idade fora da faixa aceitável para cotação.");
                }
            }
        }
    }
}
