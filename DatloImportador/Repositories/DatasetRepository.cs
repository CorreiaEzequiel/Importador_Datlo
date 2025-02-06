using DatloImportador.Data;
using DatloImportador.Models;
using Microsoft.EntityFrameworkCore;

namespace DatloImportador.Repositories
{
    public class DatasetRepository : IDatasetRepository
    {
        private readonly AppDbContext _contexto;

        public DatasetRepository(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Dataset>> ObterTodos()
        {
            return await _contexto.Datasets
                 .Include(d => d.Colunas)
                 .Include(d => d.Registros)
                     .ThenInclude(r => r.Valores)
                 .ToListAsync();
        }

        public async Task<Registro> ObterRegistroPorId(int id)
        {
            return await _contexto.Registros
         .Include(r => r.Valores)
         .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Registro>> ObterValoresPorIdentificador(IEnumerable<string> ids)
        {
            return await _contexto.Registros
          .Where(x => x.Valores.Any(val => val.Chave == "Identificador" && ids.Contains(val.Valor)))
          .Include(x => x.Valores)
          .ToListAsync();
        }

        public async Task<IEnumerable<Registro>> FiltrarPorColuna(string nomeColuna, string valorBusca)
        {
            return await _contexto.Registros
        .Where(x => x.Valores.Any(val => val.Chave == nomeColuna && val.Valor.Contains(valorBusca)))
        .Include(x => x.Valores)
        .ToListAsync();
        }

        public async Task Adicionar(Dataset dataset)
        {
            await _contexto.Datasets.AddAsync(dataset);
            await _contexto.SaveChangesAsync();
        }

        public async Task AdicionarRegistro(Registro registro)
        {
            await _contexto.Registros.AddAsync(registro);
            await _contexto.SaveChangesAsync();
        }

        public async Task Atualizar(Dataset dataset)
        {
            _contexto.Datasets.Update(dataset);
            await _contexto.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var registro = await _contexto.Registros
        .Include(r => r.Valores)
        .FirstOrDefaultAsync(r => r.Id == id);

            if (registro != null)
            {
                _contexto.Registros.Remove(registro);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}

