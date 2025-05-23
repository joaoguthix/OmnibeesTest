using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration.Context
{
    public abstract class BaseContextSqlServer : DbContext
    {
        protected BaseContextSqlServer(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<FaixaIdade> FaixaIdade { get; set; }
        public DbSet<TipoParentesco> TipoParentesco { get; set; }
        public DbSet<Parceiro> Parceiro { get; set; }
        public DbSet<Cotacao> Cotacao { get; set; }
        public DbSet<Cobertura> Cobertura { get; set; }
        public DbSet<CotacaoCobertura> CotacaoCobertura { get; set; }
        public DbSet<CotacaoBeneficiario> CotacaoBeneficiario { get; set; }
    }
}
