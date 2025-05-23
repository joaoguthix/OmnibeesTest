using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiOmnibess.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class CotacaoBeneficiarioController : ControllerBase
    {
        private readonly ICotacaoBeneficiarioAppService _cotacaoAppService;

        public CotacaoBeneficiarioController(ICotacaoBeneficiarioAppService cotacaoAppService)
        {
            _cotacaoAppService = cotacaoAppService;
        }

        [HttpPut]
        [Route("AtualizarBeneficiariosAsync")]
        public async Task<IActionResult> AtualizarBeneficiariosAsync([FromBody] List<CotacaoBeneficiario> novosBeneficiarios, [FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            try
            {
                await _cotacaoAppService.AtualizarCotacaoBeneficiariosAsync(idCotacao, secret, novosBeneficiarios);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpDelete]
        [Route("RemoverBeneficiarioAsync")]
        public async Task<IActionResult> RemoverBeneficiarioAsync([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret, [FromQuery][Required] int removeBeneficiarioId)
        {
            try
            {
                await _cotacaoAppService.RemoverCotacaoBeneficiarioAsync(idCotacao, secret, removeBeneficiarioId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet]
        [Route("ListarBeneficiariosPorCotacaoAsync")]
        public async Task<IActionResult> ListarBeneficiariosPorCotacaoAsync([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            try
            {
                var response = await _cotacaoAppService.ListarBeneficiariosPorCotacaoAsync(idCotacao, secret);
                if (response == null || !response.Any())
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet]
        [Route("DetalharBeneficiarioAsync")]
        public async Task<IActionResult> DetalharBeneficiarioAsync([FromQuery][Required] int idCotacao, [FromQuery][Required] string secret)
        {
            try
            {
                var response = await _cotacaoAppService.DetalharBeneficiarioAsync(idCotacao, secret);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
