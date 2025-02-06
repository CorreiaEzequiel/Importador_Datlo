using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entities.Models
{
    public class Registro
    {
        public int Id { get; set; }
        [ForeignKey("Dataset")]
        public int DatasetId { get; set; }
        public List<ValorRegistro> Valores { get; set; } = new List<ValorRegistro>();
    }
}
