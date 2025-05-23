using Dapper;
using Domain.Entities;
using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;

namespace Infrastructure.Repository.CotacaoRepositories
{
    public class CotacaoCoberturaRepository : ICotacaoCoberturaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _uow;

        public CotacaoCoberturaRepository(
                                  IDbConnectionFactory connectionFactory,
                                  IUnitOfWork uow)
        {
            _connectionFactory = connectionFactory;
            _uow = uow;
        }
        public async Task<int> AddCotacaoCoberturaAsync(CotacaoCobertura cotacaoCobertura)
        {
            var sql = @"
                    INSERT INTO CotacaoCobertura (
                        IdCotacao, IdCobertura, ValorDesconto, ValorAgravo, ValorTotal
                    )
                    VALUES (
                        @IdCotacao, @IdCobertura, @ValorDesconto, @ValorAgravo, @ValorTotal
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int)";

            var conn = _uow.Connection;
            var transaction = _uow.Transaction;

            var idCotacaoCobertura = await conn.QuerySingleAsync<int>(sql, new
            {
                IdCotacao = cotacaoCobertura.IdCotacao,
                IdCobertura = cotacaoCobertura.IdCobertura,
                ValorDesconto = cotacaoCobertura.ValorDesconto,
                ValorAgravo = cotacaoCobertura.ValorAgravo,
                ValorTotal = cotacaoCobertura.ValorTotal
            }, transaction: transaction);

            return idCotacaoCobertura;
        }
        public async Task<bool> UpdateCotacaoCoberturaAsync(CotacaoCobertura cotacaoCobertura)
        {
            var sql = @"
                    UPDATE CotacaoCobertura
                    SET ValorDesconto = @ValorDesconto,
                        ValorAgravo = @ValorAgravo,
                        ValorTotal = @ValorTotal
                    WHERE IdCotacao = @IdCotacao
                      AND IdCobertura = @IdCobertura;";
            var conn = _uow.Connection;
            var transaction = _uow.Transaction;
            var rowsAffected = await conn.ExecuteAsync(sql, new
            {
                IdCotacao = cotacaoCobertura.IdCotacao,
                IdCobertura = cotacaoCobertura.IdCobertura,
                ValorDesconto = cotacaoCobertura.ValorDesconto,
                ValorAgravo = cotacaoCobertura.ValorAgravo,
                ValorTotal = cotacaoCobertura.ValorTotal
            }, transaction: transaction);
            return rowsAffected > 0;
        }
        public async Task<List<CotacaoCobertura>> GetCotacaoCoberturaByIdCotacaoAsync(int idCotacao, int idParceiro)
        {
            var sql = @"SELECT * 
                        FROM CotacaoCobertura cc
                        WHERE cc.IdCotacao = @IdCotacao
                          AND EXISTS (
                              SELECT 1 FROM Cotacao c 
                              WHERE c.Id = cc.IdCotacao AND c.IdParceiro = @IdParceiro
                          );";

            using var conn = _connectionFactory.GetReadConnection();

            var cotacaoCoberturas = await conn.QueryAsync<CotacaoCobertura>(
                sql,
                new { IdCotacao = idCotacao, IdParceiro = idParceiro }
            );

            return cotacaoCoberturas.ToList();
        }

        public async Task<bool> RemoveCotacaoCoberturaAsync(int idCotacaoCobertura)
        {
            var sql = "DELETE FROM CotacaoCobertura WHERE Id = @Id";
            var conn = _uow.Connection;
            var transaction = _uow.Transaction;
            var rowsAffected = await conn.ExecuteAsync(sql, new { Id = idCotacaoCobertura }, transaction: transaction);
            return rowsAffected > 0;
        }
    }
}
