using CsvHelper;
using Dominio.Entities.DTOs;
using Dominio.Entities.Models;
using Dominio.Interfaces.Data;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Dominio.Services
{
    public class ImportacaoService : IImportacaoService
    {
        private readonly IDatasetRepository _datasetRepository;

        public ImportacaoService(IDatasetRepository datasetRepository)
        {
            _datasetRepository = datasetRepository;
        }


        public async Task<ImportacaoResultadoDTO>ImportarCsv(IFormFile arquivoCsv)
        {
            var resultado = new ImportacaoResultadoDTO();

            if (arquivoCsv.Length == 0)
            {
                resultado.Erros.Add("O arquivo CSV está vazio.");
                return resultado;
            }

            using var stream = new MemoryStream();
            await arquivoCsv.CopyToAsync(stream);
            stream.Position = 0;

            using var leitor = new StreamReader(stream);
            using var csv = new CsvReader(leitor, CultureInfo.InvariantCulture);

            if (!csv.Read() || !csv.ReadHeader())
            {
                resultado.Erros.Add("O arquivo não contém cabeçalho válido.");
                return resultado;
            }

            var colunas = new List<Coluna>();
            var nomeDataset = Path.GetFileNameWithoutExtension(arquivoCsv.FileName);
            var datasetsExistentes = await _datasetRepository.ObterTodos() ?? new List<Dataset>();

            foreach (var nome in csv.HeaderRecord)
            {
                var tipo = "String";
                if (csv.TryGetField(nome, out string primeiroValor))
                {
                    if (int.TryParse(primeiroValor, out _))
                        tipo = "Int";
                    else if (DateTime.TryParse(primeiroValor, out _))
                        tipo = "DateTime";
                }
                colunas.Add(new Coluna { Nome = nome, Tipo = tipo });
            }

            var dataset = new Dataset { Nome = nomeDataset, Colunas = colunas, Registros = new List<Registro>() };

            while (csv.Read())
            {
                var registro = new Registro { Valores = new List<ValorRegistro>() };

                foreach (var coluna in colunas)
                {
                    var valor = csv.GetField(coluna.Nome);
                    if (string.IsNullOrWhiteSpace(valor))
                    {
                        resultado.RegistrosIgnorados++;
                        continue;
                    }

                    var valorRegistro = new ValorRegistro { Chave = coluna.Nome, Valor = valor };
                    var registroExistente = datasetsExistentes
                        .SelectMany(d => d.Registros)
                        .FirstOrDefault(r => r.Valores.Any(v => v.Chave == coluna.Nome && v.Valor == valor));

                    if (registroExistente != null)
                    {
                        valorRegistro.RegistroId = registroExistente.Id;
                        registroExistente.Valores.Add(valorRegistro);
                        resultado.RegistrosIgnorados++;
                    }
                    else
                    {
                        registro.Valores.Add(valorRegistro);
                    }
                }

                if (registro.Valores.Count > 0)
                {
                    dataset.Registros.Add(registro);
                    resultado.RegistrosImportados++;
                }
            }

            await _datasetRepository.Adicionar(dataset);

            return resultado;
        }

        public async Task<IEnumerable<string>> ExtrairIdentificador(IFormFile arquivoCsv)
        {
            var identificadores = new List<string>();

            using var stream = new MemoryStream();
            await arquivoCsv.CopyToAsync(stream);
            stream.Position = 0;

            using var leitor = new StreamReader(stream);
            using var csv = new CsvReader(leitor, CultureInfo.InvariantCulture);

            if (csv.Read() && csv.ReadHeader())
            {
                while (csv.Read())
                {
                    if (csv.TryGetField<string>("Identificador", out var identificador) && !string.IsNullOrWhiteSpace(identificador))
                    {
                        identificadores.Add(identificador);
                    }
                }
            }

            return identificadores;
        }


    }
}
