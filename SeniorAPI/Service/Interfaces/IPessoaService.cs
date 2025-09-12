using SeniorAPI.Enumerador;
using SeniorAPI.Model;

namespace SeniorAPI.Service.Interfaces
{
    public interface IPessoaService
    {
        PessoaModel Adicionar(PessoaModel pessoaModel);
        List<PessoaModel> ObterPessoas();
        PessoaModel? ObterPessoaPorCodigo(int codigo);
        List<PessoaModel> ObterPessoasPorUF(UF UF);
        void DeletarPessoa(int codigo);
        PessoaModel EditarPessoa(int codigo, PessoaModel pessoaModel);
    }
}