using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Backed.Models;
using Backend.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public static class TokenServices
{

    /*
    Essa classe é utilizada para algumas funções na autenticação.
    */
    public static Backend.Authentication.Token GenerateToken(IConfiguration config, int userId)
    {
        /*
        Método utilizado para gerar um token JWT
        */
        var tokenHandler = new JwtSecurityTokenHandler();

        // Buscamos a chave no arquivo de configuração
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

        // Setamos as credenciais de encriptação
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Aqui geramos uma descrição de como será o Token
        JwtSecurityToken token = new JwtSecurityToken(
            // Emissor
            issuer: config["TokenConfiguration:Issuer"],
            audience: config["TokenConfiguration:Audience"],
            // Quando o token expira
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: credentials
        );

        // E retornamos em uma classe para facilitar o código
        return new Token()
        {
            TokenAuth = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = userId
        };
    }
}