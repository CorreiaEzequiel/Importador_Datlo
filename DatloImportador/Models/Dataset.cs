using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;

namespace DatloImportador.Models
{
    public class Dataset
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Coluna> Colunas { get; set; } = new List<Coluna>();
        public List<Registro> Registros { get; set; } = new List<Registro>();
    }
}
