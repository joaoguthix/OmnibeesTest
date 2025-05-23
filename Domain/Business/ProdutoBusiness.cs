using Domain.Entities;
using Domain.Interfaces.InterfaceBusiness;
using Domain.Interfaces.InterfaceRepository;

namespace Domain.Business
{
    public class ProdutoBusiness : IProdutoBusiness
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoBusiness(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        //public async Task<IEnumerable<Produto>> ObterTodosAsync()
        //{
        //    return await _produtoRepository.ObterTodosViaEFAsync();
        //}

        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            return await _produtoRepository.ObterPorIdAsync(id);
        }

        public async Task CriarAsync(Produto produto)
        {
            // Aqui poderia haver regras de negócio
            if (string.IsNullOrWhiteSpace(produto.Description))
                throw new ArgumentException("Nome do produto é obrigatório.");

            await _produtoRepository.CriarAsync(produto);
        }
    }
}
