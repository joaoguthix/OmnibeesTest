using Domain.Business;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;
using Moq;

namespace OmnibeesTest.CotacaoTests
{
    [TestFixture]
    public class CotacaoBusinessTests
    {
        private Mock<ICotacaoRepository> _cotacaoRepositoryMock;
        private Mock<IFaixaIdadeBusiness> _faixaIdadeBusinessMock;
        private Mock<IProdutoBusiness> _produtoBusinessMock;
        private Mock<IParceiroBusiness> _parceiroBusinessMock;

        private CotacaoBusiness _cotacaoBusiness;

        [SetUp]
        public void Setup()
        {
            _cotacaoRepositoryMock = new Mock<ICotacaoRepository>();
            _faixaIdadeBusinessMock = new Mock<IFaixaIdadeBusiness>();
            _produtoBusinessMock = new Mock<IProdutoBusiness>();
            _parceiroBusinessMock = new Mock<IParceiroBusiness>();

            _cotacaoBusiness = new CotacaoBusiness(
                _cotacaoRepositoryMock.Object,
                _faixaIdadeBusinessMock.Object,
                _produtoBusinessMock.Object,
                _parceiroBusinessMock.Object
            );
        }
        [Test]
        public void AddCotacaoAsync_DeveLancarExcecao_QuandoDadosInvalidos()
        {
            // Arrange
            var cotacao = new Cotacao
            {
                NomeSegurado = null,
                Nascimento = DateTime.Now.AddYears(-30),
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _cotacaoBusiness.AddCotacaoAsync(cotacao, "Secret"));
        }
        [Test]
        public async Task AddCotacaoAsync_DeveAdicionarComSucesso()
        {
            // Arrange
            var cotacao = new Cotacao
            {
                NomeSegurado = "João",
                Nascimento = DateTime.Now.AddYears(-30),
                IdProduto = 1,
                ImportanciaSegurada = 1500,
                DDD = 12,
                Telefone = 99999999
            };

            _faixaIdadeBusinessMock.Setup(f => f.GetFaixaIdadesAsync())
                .ReturnsAsync(new List<FaixaIdadeDto> { new FaixaIdadeDto { IdadeMinima = 18, IdadeMaxima = 60 } });

            _produtoBusinessMock.Setup(p => p.ObterPorIdAsync(cotacao.IdProduto))
                .ReturnsAsync(new Produto { BaseValue = 1000, Limit = 2000 });

            _cotacaoRepositoryMock.Setup(r => r.AddCotacaoAsync(cotacao)).ReturnsAsync(1);

            // Act
            var result = await _cotacaoBusiness.AddCotacaoAsync(cotacao, "Secret");

            // Assert
            Assert.AreEqual(1, result);
        }
        [Test]
        public async Task AtualizarCotacaoAsync_DeveAtualizarComSucesso()
        {
            // Arrange
            var cotacao = new Cotacao
            {
                NomeSegurado = "João",
                Nascimento = DateTime.Now.AddYears(-25),
                IdProduto = 1,
                ImportanciaSegurada = 1200,
                DDD = 12,
                Telefone = 99999999
            };

            _faixaIdadeBusinessMock.Setup(f => f.GetFaixaIdadesAsync())
                .ReturnsAsync(new List<FaixaIdadeDto> { new FaixaIdadeDto { IdadeMinima = 18, IdadeMaxima = 60 } });

            _produtoBusinessMock.Setup(p => p.ObterPorIdAsync(cotacao.IdProduto))
                .ReturnsAsync(new Produto { BaseValue = 1000, Limit = 2000 });

            // Act
            await _cotacaoBusiness.AtualizarCotacaoAsync(cotacao);

            // Assert
            _cotacaoRepositoryMock.Verify(r => r.AtualizarCotacaoAsync(cotacao), Times.Once);
        }

        [Test]
        public async Task AtualizarPremioAsync_DeveAtualizarPremioComSucesso()
        {
            // Arrange
            var cotacao = new Cotacao
            {
                Id = 1,
                IdProduto = 1
            };

            var coberturas = new List<CotacaoCobertura>
            {
                new CotacaoCobertura { ValorTotal = 50 },
                new CotacaoCobertura { ValorTotal = 100 }
            };

            _produtoBusinessMock.Setup(p => p.ObterPorIdAsync(cotacao.IdProduto))
                .ReturnsAsync(new Produto { BaseValue = 200 });

            // Act
            await _cotacaoBusiness.AtualizarPremioAsync(cotacao, coberturas);

            // Assert
            _cotacaoRepositoryMock.Verify(r =>
                r.AtualizarImportanciaPremioAsync(cotacao.Id, 200 + 50 + 100), Times.Once);
        }

        [Test]
        public async Task CalculateValuePremio_DeveRetornarValorCorreto()
        {
            // Arrange
            var cotacao = new Cotacao { IdProduto = 1 };
            var coberturas = new List<CotacaoCobertura>
            {
                new CotacaoCobertura { ValorTotal = 50 },
                new CotacaoCobertura { ValorTotal = 70 }
            };

            _produtoBusinessMock.Setup(p => p.ObterPorIdAsync(cotacao.IdProduto))
                .ReturnsAsync(new Produto { BaseValue = 300 });

            // Act
            var result = await _cotacaoBusiness.CalculateValuePremio(cotacao, coberturas);

            // Assert
            Assert.AreEqual(420, result);
        }

        [Test]
        public async Task ExcluirCotacaoAsync_DeveExcluirComSucesso()
        {
            // Arrange
            var secret = "teste";
            var parceiro = new Parceiro { Id = 1 };

            _parceiroBusinessMock.Setup(p => p.GetParceiroBySecretAsync(secret)).ReturnsAsync(parceiro);

            // Act
            await _cotacaoBusiness.ExcluirCotacaoAsync(1, secret);

            // Assert
            _cotacaoRepositoryMock.Verify(r => r.ExcluirCotacaoAsync(1, parceiro.Id), Times.Once);
        }
    }
}
