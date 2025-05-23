using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;
using Domain.ViewModel;

namespace Domain.Business
{
    public class CotacaoBeneficiarioBusiness : ICotacaoBeneficiarioBusiness
    {
        private readonly ICotacaoBeneficiarioRepository _cotacaoBeneficiarioRepository;
        private readonly IParceiroBusiness _parceiroBusiness;
        public CotacaoBeneficiarioBusiness(ICotacaoBeneficiarioRepository cotacaoBeneficiarioRepository, IParceiroBusiness parceiroBusiness)
        {
            _cotacaoBeneficiarioRepository = cotacaoBeneficiarioRepository;
            _parceiroBusiness = parceiroBusiness;
        }

        public async Task AddCotacaoBeneficiarioAsync(List<CotacaoBeneficiario> cotacaoBeneficiario, int idCotacao)
        {
            try
            {
                ValidateCotacaoBeneficiario(cotacaoBeneficiario);
                var beneficiariosOrdenados = cotacaoBeneficiario
                    .OrderBy(b => b.IdParentesco)
                    .ToList();

                foreach (var item in beneficiariosOrdenados)
                {
                    item.IdCotacao = idCotacao;
                    await _cotacaoBeneficiarioRepository.AddCotacaoBeneficiarioAsync(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AtualizarCotacaoBeneficiariosAsync(int idCotacao, string secret, List<CotacaoBeneficiario> novosBeneficiarios)
        {
            ValidateCotacaoBeneficiario(novosBeneficiarios);

            var beneficiariosOrdenados = novosBeneficiarios
                .OrderBy(b => b.IdParentesco)
                .ToList();

            var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);

            var beneficiariosAtuais = (await _cotacaoBeneficiarioRepository.GetCotacaoBeneficiarioByIdCotacaoAsync(idCotacao, parceiro.Id)).ToList();

            if (beneficiariosAtuais.Count == 0)
            {
                throw new ArgumentException("Beneficiário não encontrado.");
            }

            foreach (var ben in beneficiariosAtuais)
            {
                await _cotacaoBeneficiarioRepository.RemoverCotacaoBeneficiarioAsync(ben.Id);
            }

            foreach (var novo in beneficiariosOrdenados)
            {
                novo.IdCotacao = idCotacao;
                await _cotacaoBeneficiarioRepository.AddCotacaoBeneficiarioAsync(novo);
            }
        }


        public async Task RemoverCotacaoBeneficiarioAsync(int idCotacao, string secret, int removeBeneficiarioId)
        {
            var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
            var beneficiariosAtuais = (await _cotacaoBeneficiarioRepository.GetCotacaoBeneficiarioByIdCotacaoAsync(idCotacao, parceiro.Id)).ToList();
            if (!beneficiariosAtuais.Any())
                throw new ArgumentException("Nenhum beneficiário encontrado para esta cotação.");

            var beneficiariosRestantes = beneficiariosAtuais.Where(b => b.Id != removeBeneficiarioId).ToList();

            if (!beneficiariosRestantes.Any())
            {
                await _cotacaoBeneficiarioRepository.RemoverCotacaoBeneficiarioAsync(removeBeneficiarioId);
                return;
            }

            var somaAtual = beneficiariosRestantes.Sum(b => b.Percentual);

            foreach (var b in beneficiariosRestantes)
            {
                b.Percentual = Math.Round((b.Percentual.GetValueOrDefault() / somaAtual.GetValueOrDefault()) * 100m, 2);
            }

            var diferenca = 100m - beneficiariosRestantes.Sum(b => b.Percentual);
            if (Math.Abs(diferenca.GetValueOrDefault()) >= 0.01m)
            {
                beneficiariosRestantes[0].Percentual += diferenca;
            }

            foreach (var ben in beneficiariosAtuais)
            {
                await _cotacaoBeneficiarioRepository.RemoverCotacaoBeneficiarioAsync(ben.Id);
            }

            var beneficiariosOrdenados = beneficiariosRestantes
                .OrderBy(b => b.IdParentesco)
                .ToList();

            foreach (var novo in beneficiariosOrdenados)
            {
                novo.IdCotacao = idCotacao;
                await _cotacaoBeneficiarioRepository.AddCotacaoBeneficiarioAsync(novo);
            }
        }

        public async Task<List<CotacaoBeneficiarioDetailViewModel>> DetalharBeneficiarioAsync(int idCotacao, string secret)
        {
            var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
            var result = await _cotacaoBeneficiarioRepository.DetalharBeneficiarioAsync(idCotacao, parceiro.Id);
            if (result == null)
            {
                throw new ArgumentException("Beneficiário não encontrado.");
            }
            return result;
        }

        public async Task<List<CotacaoBeneficiarioViewModel>> ListarBeneficiariosPorCotacaoAsync(int idCotacao, string secret)
        {
            var parceiro = await _parceiroBusiness.GetParceiroBySecretAsync(secret);
            var result = await _cotacaoBeneficiarioRepository.ListarBeneficiariosPorCotacaoAsync(idCotacao, parceiro.Id);
            if (result == null)
            {
                throw new ArgumentException("Beneficiário não encontrado.");
            }
            return result;
        }

        private void ValidateCotacaoBeneficiario(List<CotacaoBeneficiario> cotacaoBeneficiario)
        {
            var totalPercentual = cotacaoBeneficiario.Sum(x => x.Percentual);

            if (totalPercentual != 100)
            {
                throw new ArgumentException("A soma do percentual de todos os beneficiários deve ser igual a 100.");
            }
        }

    }
}
