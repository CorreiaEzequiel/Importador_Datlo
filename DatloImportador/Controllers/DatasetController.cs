using DatloImportador.Services;
using Microsoft.AspNetCore.Mvc;
using DatloImportador.Models;
using DatloImportador.DTO;

namespace DatloImportador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatasetController : ControllerBase
    {
        private readonly IDatasetService _datasetService;

        public DatasetController(IDatasetService datasetsService)
        {
            _datasetService = datasetsService;
        }
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<DatasetResponseDTO>>> ConsultarTodos()
        {
            var datasets = await _datasetService.ObterTodos();
            if (datasets == null || !datasets.Any())
            {
                return NotFound("Nenhum registro encontrado.");
            }
            return Ok(datasets);
        }


        [HttpGet("registro/{id}")]
        public async Task<ActionResult<Dataset>> ConsultarPorId(int id)
        {
            var dataset = await _datasetService.ObterPorId(id);
            if (dataset == null)
            {
                return NotFound($"Registro com ID {id} não encontrado.");
            }
            return Ok(dataset);
        }


        [HttpPost("cadastrar")]
        public async Task<ActionResult<Dataset>> CriarDataSet([FromBody] Dataset dataset)
        {
            if (dataset == null)
            {
                return BadRequest("Dados inválidos.");
            }

            await _datasetService.Adicionar(dataset);

            return CreatedAtAction(nameof(ConsultarPorId), new { id = dataset.Id }, dataset);
        }

        [HttpPost("registro/cadastrar")]
        public async Task<ActionResult<RegistroResponseDTO>> AdicionarRegistro([FromBody] RegistroRequestDTO registro, int datasetId)
        {
            if (registro == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _datasetService.AdicionarRegistro(registro, datasetId);
                return Ok("Registro criado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("filtrar-por-coluna")]
        public async Task<ActionResult<IEnumerable<Registro>>> FiltrarPorColuna([FromQuery] string nomeColuna, [FromQuery] string valorBusca)
        {
            if (string.IsNullOrEmpty(nomeColuna) || string.IsNullOrEmpty(valorBusca))
            {
                return BadRequest("Os parâmetros 'nomeColuna' e 'valorBusca' são obrigatórios.");
            }

            var registros = await _datasetService.FiltrarPorColuna(nomeColuna, valorBusca);
            if (registros == null || !registros.Any())
            {
                return NotFound("Nenhum registro encontrado com os critérios fornecidos.");
            }

            return Ok(registros);
        }


        [HttpPut("{id}/atualizar")]
        public async Task<IActionResult> AtualizarDataset(int id, [FromBody] Dataset dataset)
        {
            if (dataset == null || dataset.Id != id)
            {
                return BadRequest("Dados inválidos.");
            }

            var datasetExistente = await _datasetService.ObterPorId(id);
            if (datasetExistente == null)
            {
                return NotFound($"Registro com ID {id} não encontrado.");
            }

            await _datasetService.Atualizar(dataset);
            return Ok(dataset);
        }


        [HttpDelete("{id}/deletar")]
        public async Task<IActionResult> DeletarDataset(int id)
        {
            var datasetExistente = await _datasetService.ObterPorId(id);
            if (datasetExistente == null)
            {
                return NotFound($"Dataset com ID {id} não encontrado.");
            }

            await _datasetService.Remover(id);
            return NoContent();
        }

        [HttpPost("comparar")]
        public async Task<ActionResult> CompararPokemonPorIdentificador(IFormFile file)
        {
            try
            {
                var registros = await _datasetService.ObterValoresRegistrosPorIdentificador(file);
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao processar o arquivo. Verfique se o formato é compativél.");
            }
        }
    }
}
