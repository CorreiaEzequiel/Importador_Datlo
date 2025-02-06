using System.Threading.Tasks;
using Dominio.Entities.Models;

namespace Dominio.Interfaces.Data
{
    public interface IDatasetRepository
    {
        Task<IEnumerable<Dataset>> ObterTodos();
        Task<Registro> ObterRegistroPorId(int id);
        Task<IEnumerable<Registro>> ObterValoresPorIdentificador(IEnumerable<string> ids);
        Task Adicionar(Dataset dataset);
        Task AdicionarRegistro(Registro registro);

        Task Atualizar(Dataset dataset);
        Task Remover(int id);
        Task<IEnumerable<Registro>> FiltrarPorColuna(string nomeColuna, string valorBusca);
    }
}
