using DatloImportador.Data;
using DatloImportador.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DatloImportador.Models;
using DatloImportador.DTO;

namespace DatloImportador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportadorController : ControllerBase
    {
        private readonly IImportacaoService _service;

       public ImportadorController(IImportacaoService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ImportacaoResultadoDTO>> ImportarCsv(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            try
            {
                var resultado = await _service.ImportarCsv(file);

                if (resultado.Erros.Count > 0)
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = ex.Message });
            }
        }
    }
}
