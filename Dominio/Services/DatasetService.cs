using Dominio.Entities.DTOs;
using Dominio.Entities.Models;
using Dominio.Interfaces.Data;
using Dominio.Interfaces.Services;
using Dominio.Utils;
using Microsoft.AspNetCore.Http;

namespace Dominio.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly IDatasetRepository _datasetRepository;
        private readonly IImportacaoService _importacaoService;

        public DatasetService(IDatasetRepository datasetRepository, IImportacaoService importacaoService)
        {
            _datasetRepository = datasetRepository;
            _importacaoService = importacaoService;
        }

        public async Task<IEnumerable<DatasetResponseDTO>> ObterTodos()
        {
            var listDataset = await _datasetRepository.ObterTodos();
            return listDataset.Select(x => Mapper.MapearDataSetParaDTO(x)).ToList();
        }

        public async Task<IEnumerable<RegistroResponseDTO>> ObterValoresRegistrosPorIdentificador(IFormFile file)
        {
            var listaIdentificadores = await _importacaoService.ExtrairIdentificador(file);
            var registros = await _datasetRepository.ObterValoresPorIdentificador(listaIdentificadores);
            return registros.Select(x => Mapper.MapearRegistroParaDto(x));
        }

        public async Task<RegistroResponseDTO> ObterPorId(int id)
        {
            var registro = await _datasetRepository.ObterRegistroPorId(id);
            return Mapper.MapearRegistroParaDto(registro);
        }

        public async Task Adicionar(Dataset dataset)
        {
            await _datasetRepository.Adicionar(dataset);
        }

        public async Task AdicionarRegistro(RegistroRequestDTO dto, int datasetId)
        {
            var registro = Mapper.MapearDtoParaRegistro(dto);
            registro.DatasetId = datasetId;
            await _datasetRepository.AdicionarRegistro(registro);
        }

        public async Task Atualizar(Dataset dataset)
        {
            await _datasetRepository.Atualizar(dataset);
        }

        public async Task Remover(int id)
        {
            await _datasetRepository.Remover(id);
        }

        public async Task<IEnumerable<Registro>> FiltrarPorColuna(string nomeColuna, string valorBusca)
        {
            return await _datasetRepository.FiltrarPorColuna(nomeColuna, valorBusca);
        }
    }
}

