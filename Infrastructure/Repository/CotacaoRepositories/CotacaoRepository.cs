using Dapper;
using Domain.Aggregate;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.CotacaoRepositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public CotacaoRepository(
            IDbConnectionFactory connectionFactory,
            IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }

        public async Task<int> AddCotacaoAsync(Domain.Entities.Cotacao cotacao)
        {
            var sql = @"
            INSERT INTO Cotacao (
                IdProduto, DataCriacao, DataAtualizacao, IdParceiro, NomeSegurado, 
                DDD, Telefone, Endereco, CEP, Documento, Nascimento, Premio, ImportanciaSegurada
            )
            VALUES (
                @IdProduto, @DataCriacao, @DataAtualizacao, @IdParceiro, @NomeSegurado, 
                @DDD, @Telefone, @Endereco, @CEP, @Documento, @Nascimento, @Premio, @ImportanciaSegurada
            );
            SELECT CAST(SCOPE_IDENTITY() as int)";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            var idCotacao = await conn.QuerySingleAsync<int>(sql, new
            {
                IdProduto = cotacao.IdProduto,
                DataCriacao = cotacao.DataCriacao,
                DataAtualizacao = cotacao.DataAtualizacao,
                IdParceiro = cotacao.IdParceiro,
                NomeSegurado = cotacao.NomeSegurado,
                DDD = cotacao.DDD,
                Telefone = cotacao.Telefone,
                Endereco = cotacao.Endereco,
                CEP = cotacao.CEP,
                Documento = cotacao.Documento,
                Nascimento = cotacao.Nascimento,
                Premio = cotacao.Premio,
                ImportanciaSegurada = cotacao.ImportanciaSegurada
            }, transaction: transaction);

            return idCotacao;
        }

        public async Task AtualizarImportanciaPremioAsync(int idCotacao, decimal? premio)
        {
            var sql = @"
                        UPDATE Cotacao
                        SET 
                            Premio = @Premio
                        WHERE 
                            Id = @IdCotacao;";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            await conn.ExecuteAsync(sql, new
            {
                IdCotacao = idCotacao,
                Premio = premio,
            }, transaction: transaction);
        }

        public async Task<List<CotacaoProdutoAggregate>> GetCotacoesByParceiroAsync(int idParceiro, int pageNumber, int pageSize)
        {
            var sql = @"SELECT 
                    c.Id, 
                    c.NomeSegurado, 
                    c.Documento, 
                    c.IdProduto,
                    p.Description AS NomeProduto
                FROM Cotacao c
                INNER JOIN Produto p ON c.IdProduto = p.Id
                WHERE c.IdParceiro = @IdParceiro
                ORDER BY c.Id
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;";

            var offset = (pageNumber - 1) * pageSize;

            using var conn = _connectionFactory.GetReadConnection();

            var result = await conn.QueryAsync<(int Id, string NomeSegurado, string Documento, int IdProduto, string NomeProduto)>(
                sql,
                new { IdParceiro = idParceiro, Offset = offset, PageSize = pageSize }
            );

            var aggregates = result.Select(x => new CotacaoProdutoAggregate
            {
                Id = x.Id,
                NomeSegurado = x.NomeSegurado,
                Documento = x.Documento,
                IdProduto = x.IdProduto,
                Produto = new Produto
                {
                    Description = x.NomeProduto
                }
            }).ToList();

            return aggregates;
        }

        public async Task<CotacaoAggregate?> GetCotacaoDetailsByIdAsync(int idCotacao, int idParceiro)
        {
            var sql = @"SELECT 
                    c.Id, 
                    c.IdProduto,
                    c.DataCriacao,
                    c.DataAtualizacao,
                    c.NomeSegurado,
                    c.DDD,
                    c.Telefone,
                    c.Endereco,
                    c.CEP,
                    c.Documento,
                    c.Nascimento,
                    c.Premio,
                    c.ImportanciaSegurada
                FROM Cotacao c
                WHERE c.Id = @IdCotacao AND c.IdParceiro = @IdParceiro;

                SELECT * FROM CotacaoBeneficiario WHERE IdCotacao = @IdCotacao;

                SELECT * FROM CotacaoCobertura WHERE IdCotacao = @IdCotacao;";

            using var conn = _connectionFactory.GetReadConnection();
            using var multi = await conn.QueryMultipleAsync(sql, new { IdCotacao = idCotacao, IdParceiro = idParceiro });

            var cotacao = await multi.ReadSingleOrDefaultAsync<CotacaoAggregate>();
            if (cotacao == null)
                return null;

            cotacao.CotacaoBeneficiarios = (await multi.ReadAsync<CotacaoBeneficiario>()).ToList();
            cotacao.CotacaoCoberturas = (await multi.ReadAsync<CotacaoCobertura>()).ToList();

            return cotacao;
        }

        public async Task<Cotacao?> GetCotacaoByIdAsync(int id, int idParceiro)
        {
            var sql = "SELECT * FROM Cotacao WHERE Id = @Id AND IdParceiro = @IdParceiro";
            using var conn = _connectionFactory.GetReadConnection();
            return await conn.QueryFirstOrDefaultAsync<Cotacao>(sql, new { Id = id, IdParceiro = idParceiro });
        }

        public async Task ExcluirCotacaoAsync(int idCotacao, int idParceiro)
        {
            var sql = @"DELETE FROM CotacaoBeneficiario 
                        WHERE IdCotacao = @IdCotacao 
                          AND EXISTS (SELECT 1 FROM Cotacao WHERE Id = @IdCotacao AND IdParceiro = @IdParceiro);

                        DELETE FROM CotacaoCobertura 
                        WHERE IdCotacao = @IdCotacao 
                          AND EXISTS (SELECT 1 FROM Cotacao WHERE Id = @IdCotacao AND IdParceiro = @IdParceiro);

                        DELETE FROM Cotacao 
                        WHERE Id = @IdCotacao AND IdParceiro = @IdParceiro;
                    ";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            await conn.ExecuteAsync(sql, new { IdCotacao = idCotacao, IdParceiro = idParceiro }, transaction: transaction);
        }
        public async Task AtualizarCotacaoAsync(Cotacao cotacao)
        {
            var sql = @"UPDATE Cotacao
                        SET 
                            IdProduto = @IdProduto,
                            DataAtualizacao = @DataAtualizacao,
                            NomeSegurado = @NomeSegurado,
                            DDD = @DDD,
                            Telefone = @Telefone,
                            Endereco = @Endereco,
                            CEP = @CEP,
                            Documento = @Documento,
                            Nascimento = @Nascimento,
                            Premio = @Premio,
                            ImportanciaSegurada = @ImportanciaSegurada
                        WHERE Id = @Id AND IdParceiro = @IdParceiro;
                    ";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            await conn.ExecuteAsync(sql, cotacao, transaction: transaction);
        }
    }
}