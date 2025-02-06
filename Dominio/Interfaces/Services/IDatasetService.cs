using Dominio.Entities.DTOs;
using Dominio.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace Dominio.Interfaces.Services
{
    public interface IDatasetService
    {
        Task<IEnumerable<DatasetResponseDTO>> ObterTodos();
        Task<RegistroResponseDTO> ObterPorId(int id);
        Task<IEnumerable<RegistroResponseDTO>> ObterValoresRegistrosPorIdentificador(IFormFile file);
        Task Adicionar(Dataset dataset);
        Task AdicionarRegistro(RegistroRequestDTO dto, int datasetId);
        Task Atualizar(Dataset dataset);
        Task Remover(int id);
        Task<IEnumerable<Registro>> FiltrarPorColuna(string nomeColuna, string valorBusca);
    }
}
