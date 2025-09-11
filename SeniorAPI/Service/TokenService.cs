using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeniorAPI.Service
{
    public class TokenService(string token)
    {
        private static readonly ConcurrentDictionary<string, string> _dicionarioTokensArmazenados = [];
        private readonly string _token = token;

        public string GerarToken(string usuario)
        {
            Claim[] dadosUsuario =
            [
                new Claim(ClaimTypes.Name, usuario)
            ];

            SymmetricSecurityKey chave = new(Encoding.UTF8.GetBytes(_token));
            SigningCredentials credenciais = new(chave, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtToken = new(
                                            issuer: "localhost",
                                            audience: "localhost",
                                            claims: dadosUsuario,
                                            expires: DateTime.Now.AddHours(1),
                                            signingCredentials: credenciais
                                            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            _dicionarioTokensArmazenados[usuario] = tokenString;

            return tokenString;
        }

        public bool ValidarToken(string usuario, string token) =>
           _dicionarioTokensArmazenados.TryGetValue(usuario, out string? value) && value.Equals(token);
    }
}
