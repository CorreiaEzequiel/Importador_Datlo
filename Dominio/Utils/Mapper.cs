using Dominio.Entities.DTOs;
using Dominio.Entities.Models;

namespace Dominio.Utils
{
    public static class Mapper
    {
        public static DatasetResponseDTO MapearDataSetParaDTO(Dataset dataSet)
        {
            DatasetResponseDTO dto = new DatasetResponseDTO
            {
                Id = dataSet.Id,
                Nome = dataSet.Nome,
                Colunas = dataSet.Colunas.Select(col => MapearColunaParaDto(col)).ToList() ?? new List<ColunaResponseDTO>(),
                Registros = dataSet.Registros.Select(reg => MapearRegistroParaDto(reg)).ToList() ?? new List<RegistroResponseDTO>()
            };

            return dto;
        }

        public static RegistroResponseDTO MapearRegistroParaDto(Registro registro)
        {
            IDictionary<string, string> valores = new Dictionary<string, string>();
            foreach (var valor in registro.Valores)
            {
                valores.Add(valor.Chave, valor.Valor);
            }

            RegistroResponseDTO dto = new RegistroResponseDTO
            {
                DatasetId = registro.DatasetId,
                RegistroId = registro.Id,
                Valores = valores
            };

            return dto;
        }

        public static Registro MapearDtoParaRegistro(RegistroRequestDTO registroRequestDto)
        {
            List<ValorRegistro> valoresRegistro = new List<ValorRegistro>();
            foreach (var reg in registroRequestDto.valores)
            {
                valoresRegistro.Add(new ValorRegistro
                {
                    Chave = reg.Key,
                    Valor = reg.Value
                });
            }

            return new Registro
            {
                Valores = valoresRegistro
            };
        }

        public static ColunaResponseDTO MapearColunaParaDto(Coluna coluna)
        {
            ColunaResponseDTO dto = new ColunaResponseDTO
            {
                ColunaId = coluna.Id,
                DatasetId = coluna.DatasetId,
                Nome = coluna.Nome,
                Tipo = coluna.Tipo
            };

            return dto;
        }
    }
}
