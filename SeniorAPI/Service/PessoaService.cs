using SeniorAPI.Enumerador;
using SeniorAPI.Model;
using SeniorAPI.Validacao;
using System.Text;

namespace SeniorAPI.Service
{
    public class PessoaService
    {
        private static readonly List<PessoaModel> listaPessoas = [];
        private static int _codigoInicial = 0;

        public PessoaModel Adicionar(PessoaModel pessoaModel)
        {
            if (!ValideCamposPessoa(pessoaModel, false, out string erros) && !string.IsNullOrEmpty(erros))
                throw new ArgumentException(erros);

            listaPessoas.Add(pessoaModel);

            return pessoaModel;
        }

        public PessoaModel EditarPessoa(PessoaModel pessoaModel)
        {
            if (!ValideCamposPessoa(pessoaModel, false, out string erros) && !string.IsNullOrEmpty(erros))
                throw new ArgumentException(erros);

            if(listaPessoas.Count != 0)
            {
                PessoaModel? pessoaJaCadastrada = listaPessoas.FirstOrDefault(c => Equals(c.Codigo, pessoaModel.Codigo));

                if (pessoaJaCadastrada != null)
                {
                    pessoaJaCadastrada.Nome = pessoaModel.Nome;
                    pessoaJaCadastrada.UF = pessoaModel.UF;
                    pessoaJaCadastrada.CPF = pessoaModel.CPF;
                    pessoaJaCadastrada.DataNascimento = pessoaModel.DataNascimento;
                }
            }
            else
            {
                throw new ArgumentException("Pessoa não encontrada, para realizar a edição");
            }

                return pessoaModel;
        }

        public List<PessoaModel> ObterPessoas() =>
            listaPessoas;

        public PessoaModel? ObterPessoaPorCodigo(int codigo) =>
            listaPessoas.FirstOrDefault(c => Equals(c.Codigo, codigo));

        public List<PessoaModel> ObterPessoasPorUF(UF UF) =>
            [.. listaPessoas.Where(c => Equals(c.UF, UF))];

        public void ExlcuirPessoa(int codigo)
        {
            PessoaModel? pessoaRemovida = listaPessoas.FirstOrDefault(c => c.Codigo == codigo);

            if (pessoaRemovida != null)
                listaPessoas.Remove(pessoaRemovida);
        }

        private static bool ValideCamposPessoa(PessoaModel pessoaModel, bool ehEdicao, out string errosRetornados)
        {
            errosRetornados = string.Empty;

            if (pessoaModel.Codigo <= 0)
                pessoaModel.Codigo = listaPessoas.Count != 0 ? listaPessoas.Last().Codigo++ : _codigoInicial++;

            StringBuilder erros = new();

            if (!ehEdicao && listaPessoas.Any(c => Equals(c.Codigo, pessoaModel.Codigo)))
                erros.AppendLine("Pessoa com o mesmo código já foi adicionada.");

            if (!Equals(pessoaModel.UF, 0) && !Enum.IsDefined(typeof(UF), pessoaModel.UF))
                erros.AppendLine("UF inválido");

            if (!string.IsNullOrEmpty(pessoaModel.CPF))
            {
                string mensagemErroCPF = string.Empty;
                bool ehCPFValido = new CPFValidacao().EhCPFValido(pessoaModel.CPF, out mensagemErroCPF);

                if (!ehCPFValido)
                    erros.AppendLine(mensagemErroCPF.ToString());
            }

            string mensagemErroDataNasimento = string.Empty;
           

            if (erros.Length > 0)
            {
                errosRetornados = erros.ToString();
                return false;
            }

            return true;
        }
    }
}