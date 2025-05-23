using Microsoft.Extensions.DependencyInjection;

namespace Ioc.Config
{
    public interface IContainerRegistrar
    {
        public void RegisterRepository<TRepository>(IServiceCollection services, Type repositoryType);
        public void RegisterService<TService>(IServiceCollection services, Type serviceType);
    }
}
