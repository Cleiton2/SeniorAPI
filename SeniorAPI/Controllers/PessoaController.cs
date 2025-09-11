using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorAPI.Model;
using SeniorAPI.Service;

namespace SeniorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        private readonly PessoaService _pessoaSerivce;

        public PessoaController(PessoaService pessoaService)
        {
            _pessoaSerivce = pessoaService;
        }

        [HttpPost("Adicionar")]
        public IActionResult AdicionarPessoa([FromBody] PessoaModel novaPessoa)
        {
            PessoaModel resultado = _pessoaSerivce.Adicionar(novaPessoa);

            if (resultado == null)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}