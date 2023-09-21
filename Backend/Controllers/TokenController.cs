using Backed.Models;
using Backend.Context;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Serilog;

namespace Backend.Controllers;

[Route("api/auth")]
[ApiController]
public class TokenController: ControllerBase 
{
    /*
    Fiz a injestão de dependencia de configuração para ler o appsettings,
    manetendo maior segurança no código.
    */
    private readonly AppDbContext _con;
    private readonly IConfiguration _config;
    public TokenController(AppDbContext context,IConfiguration config)
    {
        _con = context;
        _config = config;
    }

    /*
    Endpoint para registro de usuário
    */
    [HttpPost]
    public async Task<ActionResult> RegisterUser([FromBody] UserAuth user)
    {
        // Validação
        if (user is null) return BadRequest("Usuário não válido para cadastro");
        if (_con.UserAuths.Where(x => x.Name == user.Name).FirstOrDefault() is not null) return BadRequest("Já temos um usuário com esse nome");

        _con.UserAuths.Add(user);
        await _con.SaveChangesAsync();

        // Log
        Log.Information($"Um usuário de nome: {user.Name} se registrou.");

        return Ok($"Usuário {user.Name} criado com sucesso");
    }

    /*
    Endpoint para "login" ou autenticação de um usuário
    */
    [HttpPost("login")]
    public ActionResult AuthenticateUser([FromBody] UserAuth user)
    {
        // Validações
        if (user is null) return BadRequest("Usuário não válido para autenticação");
        if (_con.UserAuths.Where(x => x.Name == user.Name).FirstOrDefault() is null) return BadRequest("Esse usuário não existe");

        // Gerar o token
        var token = TokenServices.GenerateToken(_config, user.Id);

        // Log
        Log.Information($"Um usuário de nome: {user.Name} gerou um token.");

        return Ok(token);
    }
}