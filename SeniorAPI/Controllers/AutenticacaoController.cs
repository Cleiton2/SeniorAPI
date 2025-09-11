using Microsoft.AspNetCore.Mvc;
using SeniorAPI.Model;
using SeniorAPI.Service;

namespace SeniorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController(TokenService tokenService) : ControllerBase
    {
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Usuario) || string.IsNullOrEmpty(loginModel.Senha))
                return Unauthorized("Informe usuário e senha");

            if (Equals(loginModel.Usuario, "admin") && Equals(loginModel.Senha, "ADM1"))
                return Ok(new { Token = _tokenService.GerarToken(loginModel.Usuario) });

            return Unauthorized("Credenciais inválidas");
        }
    }
}