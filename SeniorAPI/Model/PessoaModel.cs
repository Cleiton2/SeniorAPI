using Newtonsoft.Json;
using SeniorAPI.Enumerador;
using SeniorAPI.Helpers;

namespace SeniorAPI.Model
{
    public class PessoaModel
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public string? CPF { get; set; }

        [JsonConverter(typeof(UFSiglaConverter))]
        public UF UF { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DataNascimento { get; set; }
    }
}