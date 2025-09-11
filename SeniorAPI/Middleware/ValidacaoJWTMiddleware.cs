using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using SeniorAPI.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SeniorAPI.Middleware
{
    public class ValidacaoJWTMiddleware(RequestDelegate next, TokenService tokenService)
    {
        private readonly RequestDelegate _next = next;
        private readonly TokenService _tokenService = tokenService;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/api/Autenticacao/login"))
            {
                await _next(httpContext);
                return;
            }

            if (httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader))
            {
                var token = authorizationHeader.FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync("Token não enviado.");
                    return;
                }

                ClaimsPrincipal? principal = ValidateTokenAndExtractClaims(token);
                if (principal == null)
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Token inválido ou expirado.");
                    return;
                }

                string? usuario = principal.Identity?.Name;
                if (usuario == null || !_tokenService.ValidarToken(usuario, token))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Token inválido ou expirado.");
                    return;
                }

                httpContext.User = principal;
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Token não enviado.");
                return;
            }

            await _next(httpContext);
        }

        private static ClaimsPrincipal? ValidateTokenAndExtractClaims(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken == null)
                {
                    return null;
                }

                ClaimsIdentity identity = new ClaimsIdentity(jwtToken.Claims, JwtBearerDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
