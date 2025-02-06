using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DatloImportador.Models
{
    public class Coluna
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        [ForeignKey("Dataset")]
        public int DatasetId { get; set; }
    }
}
