using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Domain.ViewModel;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Infrastructure.Repository.CotacaoRepositories
{
    public class CotacaoBeneficiarioRepository : ICotacaoBeneficiarioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public CotacaoBeneficiarioRepository(IDbConnectionFactory connectionFactory,
                                             IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }

        public async Task<int> AddCotacaoBeneficiarioAsync(CotacaoBeneficiario cotacaoBeneficiario)
        {
            var sql = @"
                    INSERT INTO CotacaoBeneficiario (
                        IdCotacao, IdParentesco, Nome, Percentual
                    )
                    VALUES (
                        @IdCotacao, @IdParentesco, @Nome, @Percentual
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int)";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            var idCotacaoBeneficiario = await conn.QuerySingleAsync<int>(sql, new
            {
                IdCotacao = cotacaoBeneficiario.IdCotacao,
                IdParentesco = cotacaoBeneficiario.IdParentesco,
                Nome = cotacaoBeneficiario.Nome,
                Percentual = cotacaoBeneficiario.Percentual
            }, transaction: transaction);

            return idCotacaoBeneficiario;
        }

        public async Task<List<CotacaoBeneficiario>> GetCotacaoBeneficiarioByIdCotacaoAsync(int idCotacao, int idParceiro)
        {
            var sql = @"SELECT b.*
                        FROM CotacaoBeneficiario b
                        WHERE b.IdCotacao = @IdCotacao
                          AND EXISTS (
                              SELECT 1 
                              FROM Cotacao c
                              WHERE c.Id = b.IdCotacao 
                                AND c.IdParceiro = @IdParceiro
                          );
                    ";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            var result = await conn.QueryAsync<CotacaoBeneficiario>(
                sql,
                new { IdCotacao = idCotacao, IdParceiro = idParceiro },
                transaction: transaction
            );

            return result.ToList();
        }

        public async Task RemoverCotacaoBeneficiarioAsync(int idBeneficiario)
        {
            var sql = @"DELETE FROM CotacaoBeneficiario WHERE Id = @Id;";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            await conn.ExecuteAsync(
                sql,
                new { Id = idBeneficiario },
                transaction: transaction
            );
        }

        public async Task<List<CotacaoBeneficiarioViewModel>> ListarBeneficiariosPorCotacaoAsync(int idCotacao, int idParceiro)
        {
            var sql = @"SELECT
                    b.Nome AS NomeBeneficiario,
                    tp.Description AS TipoParentesco,
                    b.Percentual AS PercentualParticipacao
                FROM CotacaoBeneficiario b
                INNER JOIN TipoParentesco tp ON b.IdParentesco = tp.Id
                WHERE b.IdCotacao = @IdCotacao
                  AND EXISTS (SELECT 1 FROM Cotacao c WHERE c.Id = b.IdCotacao AND c.IdParceiro = @IdParceiro);";

            using var conn = _connectionFactory.GetReadConnection();

            var result = await conn.QueryAsync<CotacaoBeneficiarioViewModel>(
                sql,
                new { IdCotacao = idCotacao, IdParceiro = idParceiro });

            return result.ToList();
        }

        public async Task<List<CotacaoBeneficiarioDetailViewModel>> DetalharBeneficiarioAsync(int idCotacao, int idParceiro)
        {
            var sql = @"SELECT 
                    b.Id,
                    b.IdCotacao,
                    b.Nome,
                    b.Percentual,
                    tp.Description AS TipoParentesco
                FROM CotacaoBeneficiario b
                INNER JOIN TipoParentesco tp ON b.IdParentesco = tp.Id
                WHERE b.IdCotacao = @IdCotacao
                  AND EXISTS (SELECT 1 FROM Cotacao c WHERE c.Id = b.IdCotacao AND c.IdParceiro = @IdParceiro);";

            using var conn = _connectionFactory.GetReadConnection();

            var result = await conn.QueryAsync<CotacaoBeneficiarioDetailViewModel>(
                sql,
                new { IdCotacao = idCotacao, IdParceiro = idParceiro });

            return result.ToList();
        }
    }
}