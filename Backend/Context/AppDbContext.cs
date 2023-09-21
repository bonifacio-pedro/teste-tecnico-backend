using Backed.Models;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Backend.Context;

public class AppDbContext: DbContext
{
    /*
    Esse é o contexto de conexão para o banco, onde passamos as informações (Como por exemplo
    as tabelas que serão utilizadas - DbSets) e utilizamos para se conectar ao banco.
    */
    public AppDbContext(DbContextOptions<AppDbContext> opt): base(opt)
    { }

    public DbSet<Product> Products { get; set; }
    public DbSet<UserAuth> UserAuths { get; set; }
}