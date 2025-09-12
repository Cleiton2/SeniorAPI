using Microsoft.AspNetCore.Mvc;
using Moq;
using SeniorAPI.Controllers;
using SeniorAPI.Enumerador;
using SeniorAPI.Model;
using SeniorAPI.Service.Interfaces;

namespace SeniorAPI.Testes.ControllerTest
{
    public class PessoaControllerTests
    {
        [Fact]
        public void AdicionePessoa_DeveRetornarOkComPessoa()
        {
            // Arrange
            PessoaModel pessoa = new() { Nome = "Carlos", CPF = "86004014028", UF = UF.MG, DataNascimento = DateTime.Parse("27/12/1999") };
            Mock<IPessoaService> mockService = new();
            mockService.Setup(s => s.Adicionar(It.IsAny<PessoaModel>())).Returns(pessoa);

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.AdicionePessoa(pessoa);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            PessoaModel returnPessoa = Assert.IsType<PessoaModel>(okResult.Value);
            Assert.Equal("Carlos", returnPessoa.Nome);
        }

        [Fact]
        public void ConsultePessoas_DeveRetornarLista()
        {
            // Arrange
            var pessoas = new List<PessoaModel>
            {
                new() { Nome = "Ana", UF = UF.SP },
                new() { Nome = "Pedro", UF = UF.MG }
            };
            Mock<IPessoaService> mockService = new();
            mockService.Setup(s => s.ObterPessoas()).Returns(pessoas);

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.ConsultePessoas();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<PessoaModel> lista = Assert.IsType<List<PessoaModel>>(okResult.Value);
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void ConsultePessoa_QuandoExiste_DeveRetornarPessoa()
        {
            // Arrange
            PessoaModel pessoa = new() { Nome = "João", UF = UF.SP };
            Mock<IPessoaService> mockService = new Mock<IPessoaService>();
            mockService.Setup(s => s.ObterPessoaPorCodigo(1)).Returns(pessoa);

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.ConsultePessoa(1);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            PessoaModel returnPessoa = Assert.IsType<PessoaModel>(okResult.Value);
            Assert.Equal("João", returnPessoa.Nome);
        }

        [Fact]
        public void ConsultePessoa_QuandoNaoExiste_DeveRetornarMensagem()
        {
            // Arrange
            Mock<IPessoaService> mockService = new();
            mockService.Setup(s => s.ObterPessoaPorCodigo(999)).Returns((PessoaModel?)null);

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.ConsultePessoa(999);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pessoa não encontrada", okResult.Value);
        }

        [Fact]
        public void EditePessoa_DeveRetornarOkComPessoaAtualizada()
        {
            // Arrange
            PessoaModel pessoaEditada = new() { Nome = "Maria Silva", UF = UF.RJ };
            Mock<IPessoaService> mockService = new();
            mockService.Setup(s => s.EditarPessoa(1, It.IsAny<PessoaModel>())).Returns(pessoaEditada);

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.EditePessoa(1, pessoaEditada);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            PessoaModel returnPessoa = Assert.IsType<PessoaModel>(okResult.Value);
            Assert.Equal("Maria Silva", returnPessoa.Nome);
        }

        [Fact]
        public void ConsultePessoasPeloUF_ComUFValida_DeveRetornarLista()
        {
            // Arrange
            List<PessoaModel> pessoasUF =
            [
                new () { Nome = "Ana", UF = UF.SP }
            ];

            Mock<IPessoaService> mockService = new ();
            mockService.Setup(s => s.ObterPessoasPorUF(UF.SP)).Returns(pessoasUF);

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.ConsultePessoasPeloUF("SP");

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            List<PessoaModel> lista = Assert.IsType<List<PessoaModel>>(okResult.Value);
            Assert.Single(lista);
            Assert.Equal("Ana", lista[0].Nome);
        }

        [Fact]
        public void ConsultePessoasPeloUF_ComUFInvalida_DeveRetornarBadRequest()
        {
            // Arrange
            Mock<IPessoaService> mockService = new();
            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.ConsultePessoasPeloUF("XX");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeletePessoa_DeveRetornarOk()
        {
            // Arrange
            Mock<IPessoaService> mockService = new();
            mockService.Setup(s => s.DeletarPessoa(1)); // Não precisa retornar nada

            PessoaController controller = new(mockService.Object);

            // Act
            IActionResult result = controller.DeletePessoa(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}