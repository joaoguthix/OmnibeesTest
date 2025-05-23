using Domain.Business;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;
using Moq;

namespace OmnibeesTest.CotacaoCoberturaTests
{
    [TestFixture]
    public class CotacaoCoberturaTest
    {
        private Mock<ICotacaoCoberturaRepository> _cotacaoCoberturaRepositoryMock;
        private Mock<ICoberturaBusiness> _coberturaBusinessMock;
        private Mock<IFaixaIdadeBusiness> _faixaIdadeBusinessMock;
        private Mock<IParceiroBusiness> _parceiroBusinessMock;
        private Mock<ICotacaoBusiness> _cotacaoBusinessMock;

        private CotacaoCoberturaBusiness _cotacaoCoberturaBusiness;

        [SetUp]
        public void Setup()
        {
            _cotacaoCoberturaRepositoryMock = new Mock<ICotacaoCoberturaRepository>();
            _coberturaBusinessMock = new Mock<ICoberturaBusiness>();
            _faixaIdadeBusinessMock = new Mock<IFaixaIdadeBusiness>();
            _parceiroBusinessMock = new Mock<IParceiroBusiness>();
            _cotacaoBusinessMock = new Mock<ICotacaoBusiness>();

            _cotacaoCoberturaBusiness = new CotacaoCoberturaBusiness(
                _cotacaoCoberturaRepositoryMock.Object,
                _coberturaBusinessMock.Object,
                _faixaIdadeBusinessMock.Object,
                _parceiroBusinessMock.Object,
                _cotacaoBusinessMock.Object
            );
        }

        [Test]
        public void AddCotacaoCoberturaAsync_DeveLancarExcecao_QuandoSemCoberturaBasica()
        {
            // Arrange
            var cotacao = new Cotacao { Id = 1, Nascimento = DateTime.Now.AddYears(-30) };
            var coberturas = new List<CotacaoCobertura>
            {
                new CotacaoCobertura { IdCobertura = 1, IdCotacao = 1 }
            };

            _coberturaBusinessMock.Setup(c => c.GetCoberturasAsync())
                .ReturnsAsync(new List<Cobertura>
                {
                    new Cobertura { Id = 1, Type = "Adicional", Value = 100 }
                });

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _cotacaoCoberturaBusiness.AddCotacaoCoberturaAsync(coberturas, cotacao));
        }

        [Test]
        public async Task AddCotacaoCoberturaAsync_DeveAdicionarComSucesso()
        {
            // Arrange
            var cotacao = new Cotacao { Id = 1, Nascimento = DateTime.Now.AddYears(-30) };
            var coberturas = new List<CotacaoCobertura>
            {
                new CotacaoCobertura { IdCobertura = 1, IdCotacao = 1 },
                new CotacaoCobertura { IdCobertura = 2, IdCotacao = 1 }
            };

            _coberturaBusinessMock.Setup(c => c.GetCoberturasAsync())
                .ReturnsAsync(new List<Cobertura>
                {
                    new Cobertura { Id = 1, Type = "Basica", Value = 100 },
                    new Cobertura { Id = 2, Type = "Adicional", Value = 50 }
                });

            _faixaIdadeBusinessMock.Setup(f => f.GetFaixaIdadesAsync())
                .ReturnsAsync(new List<FaixaIdadeDto> { new FaixaIdadeDto { IdadeMinima = 18, IdadeMaxima = 60 } });

            _cotacaoCoberturaRepositoryMock.Setup(r => r.AddCotacaoCoberturaAsync(It.IsAny<CotacaoCobertura>()))
                .ReturnsAsync(1);

            // Act
            var result = await _cotacaoCoberturaBusiness.AddCotacaoCoberturaAsync(coberturas, cotacao);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task RemoverCotacaoCoberturaAsync_DeveRemoverComSucesso()
        {
            // Arrange
            var cotacao = new Cotacao { Id = 1, Nascimento = DateTime.Now.AddYears(-30) };
            var parceiro = new Parceiro { Id = 1 };

            _parceiroBusinessMock.Setup(p => p.GetParceiroBySecretAsync("secret")).ReturnsAsync(parceiro);

            _cotacaoCoberturaRepositoryMock.Setup(r => r.GetCotacaoCoberturaByIdCotacaoAsync(cotacao.Id, parceiro.Id))
                .ReturnsAsync(new List<CotacaoCobertura>
                {
                    new CotacaoCobertura { Id = 1, IdCobertura = 1, IdCotacao = 1 },
                    new CotacaoCobertura { Id = 2, IdCobertura = 2, IdCotacao = 1 },
                    new CotacaoCobertura { Id = 3, IdCobertura = 3, IdCotacao = 1 }
                });

            _coberturaBusinessMock.Setup(c => c.GetCoberturasAsync())
                .ReturnsAsync(new List<Cobertura>
                {
                    new Cobertura { Id = 1, Type = "Basica", Value = 50 },
                    new Cobertura { Id = 2, Type = "Adicional", Value = 30 },
                    new Cobertura { Id = 3, Type = "Adicional", Value = 30 }
                });

            _cotacaoBusinessMock.Setup(c => c.GetCotacaoByIdAsync(cotacao.Id, "secret")).ReturnsAsync(cotacao);
            _cotacaoBusinessMock.Setup(c => c.CalculateValuePremio(cotacao, It.IsAny<List<CotacaoCobertura>>()))
                .ReturnsAsync(200m);

            // Act
            await _cotacaoCoberturaBusiness.RemoverCotacaoCoberturaAsync(cotacao.Id, "secret", 2);

            // Assert
            _cotacaoCoberturaRepositoryMock.Verify(r => r.RemoveCotacaoCoberturaAsync(2), Times.Once);
            _cotacaoBusinessMock.Verify(c => c.AtualizarCotacaoAsync(cotacao), Times.Once);
        }

        [Test]
        public void AddCotacaoCoberturaAsync_DeveLancarExcecao_QuandoDuplicadas()
        {
            // Arrange
            var cotacao = new Cotacao { Id = 1, Nascimento = DateTime.Now.AddYears(-30) };
            var coberturas = new List<CotacaoCobertura>
            {
                new CotacaoCobertura { IdCobertura = 1, IdCotacao = 1 },
                new CotacaoCobertura { IdCobertura = 1, IdCotacao = 1 }
            };

            _coberturaBusinessMock.Setup(c => c.GetCoberturasAsync())
                .ReturnsAsync(new List<Cobertura>
                {
                    new Cobertura { Id = 1, Type = "Basica", Value = 100 }
                });

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _cotacaoCoberturaBusiness.AddCotacaoCoberturaAsync(coberturas, cotacao));
        }
    }
}
