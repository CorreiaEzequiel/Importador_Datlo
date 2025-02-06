using DatloImportador.DTO;
using DatloImportador.Models;

namespace DatloImportador.Services;


public interface IImportacaoService
{
    Task<ImportacaoResultadoDTO> ImportarCsv(IFormFile arquivoCsv);
    Task<IEnumerable<string>> ExtrairIdentificador(IFormFile arquivoCsv);
}
