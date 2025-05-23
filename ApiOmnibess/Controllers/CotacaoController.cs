using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiOmnibess.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class CotacaoController : ControllerBase
    {
        private readonly ICotacaoAppService _cotacaoAppService;

        public CotacaoController(ICotacaoAppService cotacaoAppService)
        {
            _cotacaoAppService = cotacaoAppService;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CotacaoRequestDTO cotacao)
        {
            try
            {
                var response = await _cotacaoAppService.AddCotacaoAsync(cotacao);
                return Created("Sucess", response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarCotacaoAsync([FromBody] CotacaoUpdateRequestDTO cotacaoDto)
        {
            try
            {
                await _cotacaoAppService.AtualizarCotacaoAsync(cotacaoDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetCotacaoDetailsByIdAsync")]
        public async Task<IActionResult> GetCotacaoDetailsByIdAsync([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            var response = await _cotacaoAppService.GetCotacaoDetailsByIdAsync(idCotacao, secret);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet("GetCotacoesByParceiroAsync/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetCotacoesByParceiroAsync([FromQuery][Required] string secret, int pageNumber, int pageSize)
        {
            var response = await _cotacaoAppService.GetCotacoesByParceiroAsync(secret, pageNumber, pageSize);
            if (response == null || !response.Any())
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("ExcluirCotacaoAsync")]
        public async Task<IActionResult> ExcluirCotacaoAsync([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            try
            {
                await _cotacaoAppService.ExcluirCotacaoAsync(idCotacao, secret);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
