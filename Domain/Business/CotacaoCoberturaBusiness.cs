using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;
using Domain.Utils;
using System.Globalization;

namespace Domain.Business
{
    public class CotacaoCoberturaBusiness : ICotacaoCoberturaBusiness
    {
        private readonly ICotacaoCoberturaRepository _cotacaoCoberturaRepository;
        private readonly ICotacaoBusiness _cotacaoBusiness;
        private readonly ICoberturaBusiness _coberturaBusiness;
        private readonly IFaixaIdadeBusiness _faixaIdadeBusiness;
        private readonly IParceiroBusiness _parceiroBusiness;
        public CotacaoCoberturaBusiness(ICotacaoCoberturaRepository cotacaoCoberturaRepository, ICoberturaBusiness coberturaBusiness,
                                        IFaixaIdadeBusiness faixaIdadeBusiness, IParceiroBusiness parceiroBusiness, ICotacaoBusiness cotacaoBusiness)
        {
            _cotacaoCoberturaRepository = cotacaoCoberturaRepository;
            _coberturaBusiness = coberturaBusiness;
            _faixaIdadeBusiness = faixaIdadeBusiness;
            _parceiroBusiness = parceiroBusiness;
            _cotacaoBusiness = cotacaoBusiness;
        }

        public async Task<List<int>> AddCotacaoCoberturaAsync(List<CotacaoCobertura> cotacaoCobertura, Cotacao cotacao)
        {
            try
            {
                await ValidarCotacaoCobertura(cotacaoCobertura);
                await CalculateValueCobertura(cotacaoCobertura, cotacao);
                List<int> responseIds = new List<int>();
                foreach (var item in cotacaoCobertura)
                {
                    item.IdCotacao = cotacao.Id;
                    responseIds.Add(await _cotacaoCoberturaRepository.AddCotacaoCoberturaAsync(item));
                }
                return responseIds;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task RemoverCotacaoCoberturaAsync(int idCotacao, string secret, int removeCoberturaId)
        {
            var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);

            var coberturasAtuais = (await _cotacaoCoberturaRepository
                .GetCotacaoCoberturaByIdCotacaoAsync(idCotacao, parceiro.Id))
                .ToList();

            if (!coberturasAtuais.Any())
                throw new ArgumentException("Nenhuma cobertura encontrada.");

            var coberturaRemover = coberturasAtuais.FirstOrDefault(c => c.Id == removeCoberturaId);
            if (coberturaRemover == null)
                throw new ArgumentException("Cobertura não encontrada na cotação.");

            var coberturasRestantes = coberturasAtuais.Where(c => c.Id != removeCoberturaId).ToList();

            await ValidarCotacaoCobertura(coberturasRestantes);

            await _cotacaoCoberturaRepository.RemoveCotacaoCoberturaAsync(removeCoberturaId);

            var cotacao = await _cotacaoBusiness.GetCotacaoByIdAsync(idCotacao, secret);
            
            var novoPremio = await _cotacaoBusiness.CalculateValuePremio(cotacao, coberturasRestantes);

            cotacao.Premio = novoPremio;

            await _cotacaoBusiness.AtualizarCotacaoAsync(cotacao);
        }
        public async Task<List<CotacaoCobertura>> GetCotacaoCoberturaByIdCotacaoAsync(int idCotacao, string secret)
        {
            try
            {
                if (idCotacao == 0)
                {
                    throw new ArgumentException("Id da cotação não pode ser zero");
                }
                var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
                
                return await _cotacaoCoberturaRepository.GetCotacaoCoberturaByIdCotacaoAsync(idCotacao, parceiro.Id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task CalculateValueCobertura(List<CotacaoCobertura> cotacaoCoberturas, Cotacao cotacao)
        {
            var coberturas = await _coberturaBusiness.GetCoberturasAsync();
            var faixaIdades = await _faixaIdadeBusiness.GetFaixaIdadesAsync();

            int idadeSegurado = DateTime.Now.Year - cotacao.Nascimento.GetValueOrDefault().Year;

            var faixaIdade = faixaIdades.FirstOrDefault(f => idadeSegurado >= f.IdadeMinima && idadeSegurado <= f.IdadeMaxima);

            foreach (var item in cotacaoCoberturas)
            {
                var cobertura = coberturas.FirstOrDefault(c => c.Id == item.IdCobertura);
                if (cobertura is null) continue;

                item.ValorTotal = cobertura.Value;

                if (cobertura.Type.NormalizeToCompare() == TypeCobertura.Basica.ToString().NormalizeToCompare())
                {
                    decimal descontoPercent = ToPercent(faixaIdade.Desconto);
                    decimal agravoPercent = ToPercent(faixaIdade.Agravo);

                    decimal valorDesconto = 0m;
                    decimal valorAgravo = 0m;

                    if (descontoPercent > 0)
                    {
                        valorDesconto = Math.Round(item.ValorTotal.GetValueOrDefault() * descontoPercent / 100m, 2);
                    }
                    else if (agravoPercent > 0)
                    {
                        valorAgravo = Math.Round(item.ValorTotal.GetValueOrDefault() * agravoPercent / 100m, 2);
                    }

                    item.ValorDesconto = valorDesconto > 0 ? valorDesconto : null;
                    item.ValorAgravo = valorAgravo > 0 ? valorAgravo : null;

                    item.ValorTotal = item.ValorTotal - valorDesconto + valorAgravo;
                }
                else if (cobertura.Type.NormalizeToCompare() == TypeCobertura.Adicional.ToString().NormalizeToCompare())
                {
                    item.ValorDesconto = null;
                    item.ValorAgravo = null;
                    item.ValorTotal = item.ValorTotal;
                }
            }
        }

        private decimal ToPercent(string? clearPorcentagem)
        {
            if (string.IsNullOrWhiteSpace(clearPorcentagem)) return 0m;
            string clean = clearPorcentagem.Trim().Replace("%", string.Empty).Replace(",", ".");

            return Convert.ToDecimal(clean);
        }

        private async Task ValidarCotacaoCobertura(List<CotacaoCobertura> cotacao)
        {

            if (cotacao == null || !cotacao.Any())
            {
                throw new ArgumentNullException(nameof(cotacao), "Cobertura não pode ser nula ou vazia");
            }

            if (cotacao.Any(x => x.IdCotacao == 0))
            {
                throw new ArgumentException("Id da cotação não pode ser zero");
            }

            if (cotacao.GroupBy(x => x.IdCobertura).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("Não é permitido ter duas cotações iguais");
            }

            var coberturas = await _coberturaBusiness.GetCoberturasAsync();

            if (!coberturas.Any())
            {
                throw new Exception("Nenhuma cobertura encontrada");
            }

            foreach (var item in cotacao)
            {
                if (!coberturas.Any(x => x.Id == item.IdCobertura))
                {
                    throw new Exception($"Cobertura com Id {item.IdCobertura} não encontrada");
                }
            }

            if (!cotacao.Any(x => coberturas.Any(c => c.Id == x.IdCobertura && c.Type.NormalizeToCompare() == TypeCobertura.Basica.ToString().NormalizeToCompare())))
            {
                throw new ArgumentException("Obrigatório uma Cobertura do tipo básica");
            }


            if (!cotacao.Any(x => coberturas.Any(c => c.Id == x.IdCobertura && c.Type.NormalizeToCompare() == TypeCobertura.Adicional.ToString().NormalizeToCompare())))
            {
                throw new ArgumentException("Obrigatório ao menos uma Cobertura do tipo adicional");
            }
        }
    }
}
