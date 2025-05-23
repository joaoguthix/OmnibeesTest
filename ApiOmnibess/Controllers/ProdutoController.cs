using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiOmnibess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoAppService _appService;

        public ProdutoController(IProdutoAppService appService)
        {
            _appService = appService;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    var produtos = await _appService.ObterTodosAsync();
        //    return Ok(produtos);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var produto = await _appService.ObterPorIdAsync(id);
        //    return produto is null ? NotFound() : Ok(produto);
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            await _appService.CriarAsync(produto);
            return Ok();
        }
    }
}