using Microsoft.Extensions.Primitives;
using SeniorAPI.Service;

namespace SeniorAPI.Middleware
{
    public class ValidacaoJWTMiddleware(RequestDelegate next, TokenService tokenService)
    {
        private readonly RequestDelegate _next = next;
        private readonly TokenService _tokenService = tokenService;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Ignora a validação de token nas rotas de login
            if (httpContext.Request.Path.StartsWithSegments("/api/Autenticacao/login"))
            {
                await _next(httpContext); // Continua sem validação
                return;
            }

            // Verifica se o token está presente no cabeçalho Authorization
            if (httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader))
            {
                var token = authorizationHeader.FirstOrDefault()?.Split(" ").Last();

                // Valida o token
                if (token == null || !_tokenService.ValidarToken(token))
                {
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    await httpContext.Response.WriteAsync("Token inválido ou expirado.");
                    return;
                }
            }
            else
            {
                // Caso o token não tenha sido enviado
                httpContext.Response.StatusCode = 400; // Bad Request
                await httpContext.Response.WriteAsync("Token não enviado.");
                return;
            }

            // Chama o próximo middleware ou controller
            await _next(httpContext);
        }
    }
}
