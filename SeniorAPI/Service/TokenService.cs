using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeniorAPI.Service
{
    public class TokenService(string secretKey)
    {
        private readonly string _secretKey = secretKey;

        public string GerarToken(string usuario)
        {
            Claim[] dadosUsuario =
            [
                new(ClaimTypes.Name, usuario)
            ];

            SymmetricSecurityKey chave = new(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials credenciais = new(chave, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new(
                issuer: "localhost",
                audience: "localhost",
                claims: dadosUsuario,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credenciais
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return tokenString;
        }

        public bool ValidarToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = "localhost",
                    ValidAudience = "localhost"
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return validatedToken != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}