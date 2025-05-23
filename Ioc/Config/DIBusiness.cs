using Domain.Business;
using Domain.Interfaces.InterfaceBusiness;
using Microsoft.Extensions.DependencyInjection;

namespace Ioc.Config
{
    public static class DIBusiness
    {
        public static IServiceCollection MapBusinessDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProdutoBusiness, ProdutoBusiness>();
            services.AddScoped<ILogBusiness, LogBusiness>();
            services.AddScoped<ITipoParentescoBusiness, TipoParentescoBusiness>();
            services.AddScoped<ICoberturaBusiness, CoberturaBusiness>();
            services.AddScoped<ICotacaoBusiness, CotacaoBusiness>();
            services.AddScoped<ICotacaoBeneficiarioBusiness, CotacaoBeneficiarioBusiness>();
            services.AddScoped<ICotacaoCoberturaBusiness, CotacaoCoberturaBusiness>();
            services.AddScoped<IParceiroBusiness, ParceiroBusiness>();
            services.AddScoped<IFaixaIdadeBusiness, FaixaIdadeBusiness>();
            services.AddScoped<ITipoParentescoBusiness, TipoParentescoBusiness>();

            return services;
        }
    }
}
