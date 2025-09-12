using SeniorAPI.Model;
using SeniorAPI.Service;
using SeniorAPI.Enumerador;

namespace SeniorAPI.Testes.ServiceTest
{
    public class PessoaServiceTests
    {
        [Fact]
        public void Adicionar_DeveAdicionarPessoa()
        {
            // Arrange
            PessoaService service = new();
            PessoaModel pessoa = new()
            {
                Nome = "João Teste",
                CPF = "86004014028",
                UF = UF.SP,
                DataNascimento = DateTime.Parse("27/12/1999")
            };

            // Act
            PessoaModel result = service.Adicionar(pessoa);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("João Teste", result.Nome);
        }

        [Fact]
        public void EditarPessoa_QuandoPessoaExiste_DeveAtualizarDados()
        {
            // Arrange
            PessoaService service = new();
            PessoaModel pessoa = new() {Codigo = 1919, Nome = "Maria", CPF = "86004014028", UF = UF.RJ, DataNascimento = DateTime.Parse("27/12/1999") };
            service.Adicionar(pessoa);

            PessoaModel pessoaEditada = new() { Nome = "Maria Silva", CPF = "86004014028", UF = UF.RJ, DataNascimento = DateTime.Parse("22/07/1999") };

            // Act
            PessoaModel result = service.EditarPessoa(pessoa.Codigo, pessoaEditada);

            // Assert
            Assert.Equal("Maria Silva", result.Nome);
        }

        [Fact]
        public void EditarPessoa_QuandoPessoaNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            PessoaService service = new();
            PessoaModel pessoaEditada = new() { Nome = "Maria Silva", CPF = "86004014028", UF = UF.RJ, DataNascimento = DateTime.Parse("27/12/1999") };

            // Act & Assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() => service.EditarPessoa(999, pessoaEditada));
            Assert.Equal("Pessoa não encontrada, para realizar a edição", ex.Message);
        }

        [Fact]
        public void ObterPessoas_DeveRetornarListaDePessoas()
        {
            // Arrange
            PessoaService service = new();
            service.Adicionar(new PessoaModel {Codigo = 1515, Nome = "Ana", CPF = "86004014028", UF = UF.SP });
            service.Adicionar(new PessoaModel { Codigo = 1717, Nome = "Pedro", CPF = "86004014028", UF = UF.MG });

            // Act
            List<PessoaModel> result = service.ObterPessoas();

            // Assert
            Assert.IsType<List<PessoaModel>>(result);
            Assert.True(result.Count >= 2);
        }

        [Fact]
        public void ObterPessoaPorCodigo_QuandoPessoaExiste_DeveRetornarPessoa()
        {
            // Arrange
            PessoaService service = new();
            PessoaModel pessoa = new() {Codigo = 2 ,Nome = "João", CPF = "86004014028", UF = UF.SP };
            service.Adicionar(pessoa);

            // Act
            PessoaModel? result = service.ObterPessoaPorCodigo(pessoa.Codigo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("João", result!.Nome);
        }

        [Fact]
        public void ObterPessoaPorCodigo_QuandoNaoExiste_DeveRetornarNull()
        {
            // Arrange
            PessoaService service = new();

            // Act
            PessoaModel? result = service.ObterPessoaPorCodigo(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ObterPessoasPorUF_QuandoExistemPessoas_DeveRetornarListaFiltrada()
        {
            // Arrange
            PessoaService service = new();
            service.Adicionar(new PessoaModel { Nome = "Ana", CPF = "86004014028", UF = UF.SP });
            service.Adicionar(new PessoaModel { Nome = "Pedro", CPF = "86004014028", UF = UF.MG });

            // Act
            List<PessoaModel> result = service.ObterPessoasPorUF(UF.SP);

            // Assert
            Assert.Single(result);
            Assert.Equal("Ana", result[0].Nome);
        }

        [Fact]
        public void DeletarPessoa_QuandoPessoaExiste_DeveRemoverPessoa()
        {
            // Arrange
            PessoaService service = new();
            PessoaModel pessoa = new() { Nome = "Maria", CPF = "86004014028", UF = UF.RJ };
            service.Adicionar(pessoa);

            // Act
            service.DeletarPessoa(pessoa.Codigo);
            PessoaModel? result = service.ObterPessoaPorCodigo(pessoa.Codigo);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DeletarPessoa_QuandoPessoaNaoExiste_DeveLancarExcecao()
        {
            // Arrange
            PessoaService service = new();

            // Act & Assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() => service.DeletarPessoa(999));
            Assert.Equal("Pessoa não encontrada.", ex.Message);
        }
    }
}