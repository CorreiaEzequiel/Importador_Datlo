using Microsoft.AspNetCore.Mvc;
using Dominio.Interfaces.Services;
using Dominio.Entities.DTOs;
using Dominio.Entities.Models;

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
        /// <summary>
        /// Lista todos os registros do conjunto de dados(dataset)
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Busca o registro pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("registro/{id}")]
        public async Task<ActionResult<RegistroResponseDTO>> ConsultarPorId(int id)
        {
            var dataset = await _datasetService.ObterPorId(id);
            if (dataset == null || dataset.RegistroId == 0)
            {
                return NotFound($"Registro com ID {id} não encontrado.");
            }
            return Ok(dataset);
        }

        /// <summary>
        /// cadastra um novo conjunto de dados(dataset)
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
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

        /// <summary>
        /// cadastra um novo registro
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="datasetId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// faz um filtro pelo valor contido na coluna informada
        /// </summary>
        /// <param name="nomeColuna"></param>
        /// <param name="valorBusca"></param>
        /// <returns></returns>
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

        /// <summary>
        /// atualiza o conjunto de dados(dataset)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataset"></param>
        /// <returns></returns>
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

        /// <summary>
        /// deleta  registro por id dentro do dataset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// compara os os identificadores contidos na planilha e vincula aos dados existentes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
