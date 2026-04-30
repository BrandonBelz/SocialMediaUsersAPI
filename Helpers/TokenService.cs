using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config) => _config = config;

        public string GenerateToken(User user)
        {
            RSA privateKey = RSA.Create();
            privateKey.ImportFromPem(File.ReadAllText(_config["Jwt:PrivateKeyPath"]!));

            Claim[] claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()) };

            JwtSecurityToken token = new(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]!)),
                signingCredentials: new SigningCredentials(
                    new RsaSecurityKey(privateKey),
                    SecurityAlgorithms.RsaSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
