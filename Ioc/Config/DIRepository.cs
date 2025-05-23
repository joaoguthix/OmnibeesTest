using Domain.Interfaces.InterfaceRepository;
using Infrastructure.Configuration.Context;
using Infrastructure.Configuration.Dapper;
using Infrastructure.Configuration.UnitOfWork;
using Infrastructure.Repository.CoberturaRepositories;
using Infrastructure.Repository.CotacaoRepositories;
using Infrastructure.Repository.FaixaIdadeRepository;
using Infrastructure.Repository.Generic;
using Infrastructure.Repository.LogRepositories;
using Infrastructure.Repository.ParceiroRepository;
using Infrastructure.Repository.ProdutoRepositories;
using Infrastructure.Repository.TipoParentescoRepositories;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ioc.Config
{
    public static class DIRepository
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<WriteContextSqlServer>(options =>
            //options.UseSqlServer(configuration.GetConnectionString("WriteConnection")));

            services.AddDbContext<ReadContextSqlServer>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ReadConnection")));

            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            services.AddScoped<BaseRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IFaixaIdadeRepository, FaixaIdadeRepository>();
            services.AddScoped<ITipoParentescoRepository, TipoParentescoRepository>();
            services.AddScoped<IParceiroRepository, ParceiroRepository>();
            services.AddScoped<ICotacaoRepository, CotacaoRepository>();
            services.AddScoped<ICotacaoBeneficiarioRepository, CotacaoBeneficiarioRepository>();
            services.AddScoped<ICotacaoCoberturaRepository, CotacaoCoberturaRepository>();
            services.AddScoped<ICoberturaRepository, CoberturaRepository>();
            //Data Source = DSPC\SQLEXPRESS; Initial Catalog = OmniBees; Integrated Security = True; Encrypt = False
            return services;
        }
    }
}
