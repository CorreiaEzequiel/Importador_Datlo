namespace DatloImportador.DTO
{
    public class ImportacaoResultadoDTO
    {
        public int RegistrosImportados { get; set; }
        public int RegistrosIgnorados { get; set; }
        public List<string> Erros { get; set; }

        public ImportacaoResultadoDTO()
        {
            Erros = new List<string>();
        }
    }
}
