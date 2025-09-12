using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorAPI.Enumerador;
using SeniorAPI.Model;
using SeniorAPI.Service.Interfaces;

namespace SeniorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PessoaController(IPessoaService pessoaService) : ControllerBase
    {
        private readonly IPessoaService _pessoaSerivce = pessoaService;

        [HttpPost("adicionarPessoa")]
        public IActionResult AdicionePessoa([FromBody] PessoaModel pessoaModel)
        {
            PessoaModel pessoaAdicionada = _pessoaSerivce.Adicionar(pessoaModel);

            return Ok(pessoaAdicionada);
        }

        [HttpGet("Consultar")]
        public IActionResult ConsultePessoas()
        {
            List<PessoaModel> pessoas = _pessoaSerivce.ObterPessoas();

            return Ok(pessoas);
        }

        [HttpGet("consultarPorCodigo/{codigo:int}")]
        public IActionResult ConsultePessoa(int codigo)
        {
            PessoaModel? pessoa = _pessoaSerivce.ObterPessoaPorCodigo(codigo);

            return pessoa != null ? Ok(pessoa) : Ok("Pessoa não encontrada");
        }

        [HttpPut("editar/{codigo}")]
        public IActionResult EditePessoa(int codigo, [FromBody] PessoaModel pessoaModel)
        {
            if(pessoaModel == null)
            {
                throw new ArgumentException("Informe os dados da pessoa.");
            }

            PessoaModel pessoaEditada = _pessoaSerivce.EditarPessoa(codigo, pessoaModel);

            return Ok(pessoaEditada);
        }

        [HttpGet("consultarPorUF/{UF}")]
        public IActionResult ConsultePessoasPeloUF(string UF)
        {
            if (Enum.TryParse(UF, true, out UF ufEnum))
            {
                List<PessoaModel> pessoas = _pessoaSerivce.ObterPessoasPorUF(ufEnum);

                return Ok(pessoas);
            }
            else
            {
                return BadRequest("UF inválida.");
            }
        }

        [HttpDelete("deletar/{codigo}")]
        public IActionResult DeletePessoa(int codigo)
        {
            _pessoaSerivce.DeletarPessoa(codigo);

            return Ok();
        }
    }
}