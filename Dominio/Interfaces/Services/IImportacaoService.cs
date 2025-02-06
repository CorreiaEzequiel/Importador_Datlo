using Dominio.Entities.DTOs;
using Microsoft.AspNetCore.Http;

namespace Dominio.Interfaces.Services
{

    public interface IImportacaoService
    {
        Task<ImportacaoResultadoDTO> ImportarCsv(IFormFile arquivoCsv);
        Task<IEnumerable<string>> ExtrairIdentificador(IFormFile arquivoCsv);
    }
}