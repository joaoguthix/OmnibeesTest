using Domain.Business;
using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;
using Moq;

namespace OmnibeesTest.CotacaoBeneficiarioTests
{
    [TestFixture]
    public class CotacaoBeneficiarioBusinessTests
    {
        private Mock<ICotacaoBeneficiarioRepository> _cotacaoBeneficiarioRepositoryMock;
        private Mock<IParceiroBusiness> _parceiroBusinessMock;

        private CotacaoBeneficiarioBusiness _cotacaoBeneficiarioBusiness;

        [SetUp]
        public void Setup()
        {
            _cotacaoBeneficiarioRepositoryMock = new Mock<ICotacaoBeneficiarioRepository>();
            _parceiroBusinessMock = new Mock<IParceiroBusiness>();

            _cotacaoBeneficiarioBusiness = new CotacaoBeneficiarioBusiness(
                _cotacaoBeneficiarioRepositoryMock.Object,
                _parceiroBusinessMock.Object
            );
        }

        [Test]
        public void AddCotacaoBeneficiarioAsync_DeveLancarExcecao_QuandoPercentualInvalido()
        {
            // Arrange
            var beneficiarios = new List<CotacaoBeneficiario>
            {
                new CotacaoBeneficiario { IdParentesco = 1, Percentual = 60 },
                new CotacaoBeneficiario { IdParentesco = 2, Percentual = 30 }
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                _cotacaoBeneficiarioBusiness.AddCotacaoBeneficiarioAsync(beneficiarios, 1));
        }
        [Test]
        public async Task AddCotacaoBeneficiarioAsync_DeveAdicionarComSucesso_QuandoPercentualValido()
        {
            // Arrange
            var beneficiarios = new List<CotacaoBeneficiario>
            {
                new CotacaoBeneficiario { IdParentesco = 1, Percentual = 50 },
                new CotacaoBeneficiario { IdParentesco = 2, Percentual = 50 }
            };

            // Act
            await _cotacaoBeneficiarioBusiness.AddCotacaoBeneficiarioAsync(beneficiarios, 1);

            // Assert
            _cotacaoBeneficiarioRepositoryMock.Verify(r =>
                r.AddCotacaoBeneficiarioAsync(It.Is<CotacaoBeneficiario>(b => b.IdParentesco == 1 && b.IdCotacao == 1)), Times.Once);

            _cotacaoBeneficiarioRepositoryMock.Verify(r =>
                r.AddCotacaoBeneficiarioAsync(It.Is<CotacaoBeneficiario>(b => b.IdParentesco == 2 && b.IdCotacao == 1)), Times.Once);
        }

        [Test]
        public void AtualizarCotacaoBeneficiariosAsync_DeveLancarExcecao_QuandoBeneficiariosNaoEncontrados()
        {
            // Arrange
            var secret = "test";
            _parceiroBusinessMock.Setup(p => p.GetParceiroBySecretAsync(secret))
                .ReturnsAsync(new Domain.Entities.Parceiro { Id = 1 });

            _cotacaoBeneficiarioRepositoryMock.Setup(r =>
                r.GetCotacaoBeneficiarioByIdCotacaoAsync(1, 1)).ReturnsAsync(new List<CotacaoBeneficiario>());

            var novos = new List<CotacaoBeneficiario>
            {
                new CotacaoBeneficiario { IdParentesco = 1, Percentual = 100 }
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                _cotacaoBeneficiarioBusiness.AtualizarCotacaoBeneficiariosAsync(1, secret, novos));
        }

        [Test]
        public async Task AtualizarCotacaoBeneficiariosAsync_DeveRemoverEAdicionarNovos()
        {
            // Arrange
            var secret = "test";
            _parceiroBusinessMock.Setup(p => p.GetParceiroBySecretAsync(secret))
                .ReturnsAsync(new Domain.Entities.Parceiro { Id = 1 });

            var atuais = new List<CotacaoBeneficiario>
            {
                new CotacaoBeneficiario { Id = 1, IdParentesco = 1, Percentual = 100 }
            };

            _cotacaoBeneficiarioRepositoryMock.Setup(r =>
                r.GetCotacaoBeneficiarioByIdCotacaoAsync(1, 1)).ReturnsAsync(atuais);

            var novos = new List<CotacaoBeneficiario>
            {
                new CotacaoBeneficiario { IdParentesco = 2, Percentual = 100 }
            };

            // Act
            await _cotacaoBeneficiarioBusiness.AtualizarCotacaoBeneficiariosAsync(1, secret, novos);

            // Assert
            _cotacaoBeneficiarioRepositoryMock.Verify(r => r.RemoverCotacaoBeneficiarioAsync(1), Times.Once);
            _cotacaoBeneficiarioRepositoryMock.Verify(r =>
                r.AddCotacaoBeneficiarioAsync(It.Is<CotacaoBeneficiario>(b => b.IdParentesco == 2 && b.IdCotacao == 1)), Times.Once);
        }

        [Test]
        public async Task RemoverCotacaoBeneficiarioAsync_DeveRedistribuirPercentuaisERemover()
        {
            // Arrange
            var secret = "test";
            _parceiroBusinessMock.Setup(p => p.GetParceiroBySecretAsync(secret))
                .ReturnsAsync(new Domain.Entities.Parceiro { Id = 1 });

            var atuais = new List<CotacaoBeneficiario>
            {
                new CotacaoBeneficiario { Id = 1, Percentual = 50, IdParentesco = 1 },
                new CotacaoBeneficiario { Id = 2, Percentual = 50, IdParentesco = 2 }
            };

            _cotacaoBeneficiarioRepositoryMock.Setup(r =>
                r.GetCotacaoBeneficiarioByIdCotacaoAsync(1, 1)).ReturnsAsync(atuais);

            // Act
            await _cotacaoBeneficiarioBusiness.RemoverCotacaoBeneficiarioAsync(1, secret, 1);

            // Assert
            _cotacaoBeneficiarioRepositoryMock.Verify(r => r.RemoverCotacaoBeneficiarioAsync(It.IsAny<int>()), Times.Exactly(2));

            _cotacaoBeneficiarioRepositoryMock.Verify(r =>
                r.AddCotacaoBeneficiarioAsync(It.Is<CotacaoBeneficiario>(b => b.Percentual == 100)), Times.Once);
        }
    }
}
