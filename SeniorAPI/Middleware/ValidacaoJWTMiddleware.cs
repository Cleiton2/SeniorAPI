using Microsoft.Extensions.Primitives;
using SeniorAPI.Service;

namespace SeniorAPI.Middleware
{
    public class ValidacaoJWTMiddleware(RequestDelegate requestDelegate, TokenService tokenService)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;
        private readonly TokenService _tokenService = tokenService;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader))
            {
                string? token = authorizationHeader.FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    string? usuario = httpContext.User?.Identity?.Name;

                    if (usuario != null && !_tokenService.ValidarToken(usuario, token))
                    {
                        httpContext.Response.StatusCode = 401;

                        await httpContext.Response.WriteAsync("Token inválido ou expirado.");
                        return;
                    }
                }
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Token não enviado");
                return;
            }

            await _requestDelegate(httpContext);
        }
    }
}