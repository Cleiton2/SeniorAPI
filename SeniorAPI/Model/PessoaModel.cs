using SeniorAPI.Enum;

namespace SeniorAPI.Model
{
    public class PessoaModel
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public string? CPF { get; set; }
        public UF UF { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}