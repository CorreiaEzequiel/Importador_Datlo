using DatloImportador.Models;

namespace DatloImportador.DTO
{
    public class DatasetResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<ColunaResponseDTO> Colunas { get; set; } = new List<ColunaResponseDTO>();
        public ICollection<RegistroResponseDTO> Registros { get; set; } = new List<RegistroResponseDTO>();
    }
}
