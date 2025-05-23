using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiOmnibess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotacaoCoberturaController : ControllerBase
    {
        private readonly ICotacaoCoberturaAppService _cotacaoCoberturaAppService;

        public CotacaoCoberturaController(ICotacaoCoberturaAppService cotacaoCoberturaAppService)
        {
            _cotacaoCoberturaAppService = cotacaoCoberturaAppService;

        }

        [HttpPost("AddCotacaoCobertura")]
        public async Task<IActionResult> AddCotacaoCobertura([FromBody][Required] List<CotacaoCobertura> cotacaoCobertura, [FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            try
            {
                await _cotacaoCoberturaAppService.AddNovaCotacaoCoberturaAsync(cotacaoCobertura, idCotacao, secret);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoverCotacaoCobertura")]
        public async Task<IActionResult> RemoverCotacaoCobertura([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret, [FromQuery][Required] int removeCoberturaId)
        {
            try
            {
                await _cotacaoCoberturaAppService.RemoverCotacaoCoberturaAsync(idCotacao, secret, removeCoberturaId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCotacaoCoberturaByIdCotacao")]
        public async Task<IActionResult> GetCotacaoCoberturaByIdCotacao([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            try
            {
                var result = await _cotacaoCoberturaAppService.GetCotacaoCoberturaByIdCotacaoAsync(idCotacao, secret);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
