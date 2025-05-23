using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ioc.Config
{
    public static class DIServices
    {
        public static IServiceCollection MapDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<ICotacaoAppService, CotacaoAppService>();
            //services.AddScoped<IFaixaIdadeAppService, FaixaIdadeAppService>();
            //services.AddScoped<ITipoParentescoAppService, TipoParentescoAppService>();
            //services.AddScoped<ILogAppService, LogAppService>();
            //services.AddScoped<ICoberturaAppService, CoberturaAppService>();
            //services.AddScoped<IParceiroAppService, ParceiroAppService>();
            services.AddScoped<ICotacaoBeneficiarioAppService, CotacaoBeneficiarioAppService>();
            services.AddScoped<ICotacaoCoberturaAppService, CotacaoCoberturaAppService>();

            return services;
        }
    }
}
