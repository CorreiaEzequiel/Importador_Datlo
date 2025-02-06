namespace Dominio.Entities.DTOs
{
    public class RegistroResponseDTO
    {
        public int DatasetId { get; set; }
        public int RegistroId { get; set; }
        public IDictionary<string, string> Valores { get; set; }
    }
}
