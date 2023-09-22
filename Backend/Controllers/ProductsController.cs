using Backend.Context;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Backend.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController: ControllerBase
{
    /*
    No construtor da classe fazemos a injestão de dependencia do contexto para conexão
    de banco de dados, fazendo a conexão de forma única e fluída.
    */
    private readonly AppDbContext _con;
    public ProductsController(AppDbContext context,IConfiguration config)
    {
        _con = context;
    }
    

    /*
    Aqui fiz o método GET, retornando um IEnumerable (uma lista em JSON) e um ActionResult,
    que é um código HTTP, inclusive utilizando programação assíncrona para melhor fluídez.
    E fiz algumas verifacações.
    */
    [HttpGet]
    // Este método todos os usuários podem ver, mesmo os não autenticados
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await _con.Products.ToListAsync();

        if (products is null) return NotFound("Nenhum produto encontrado");

        // Log
        Log.Information($"Todos os produtos buscados.");

        return Ok(products);
    }


    /*
    Neste método retorno apenas um produto, pedindo o ProductID dele
    */
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetOneProduct([FromRoute] long id)
    {
        // Procuramos e fazemos uma validação.
        var product = await _con.Products.FindAsync(Convert.ToInt32(id));

        if (product is null) return NotFound("Nenhum produto encontrado");

        // Log
        Log.Information($"Um produto de nome: {product.Name} foi buscado por.");

        return Ok(product);
    }

    /*
    Este método será utilizado para fazer pesquisas a partir de uma barra de pesquisa
    com o nome do produto
    */
    [HttpPost("search")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<Product>> GetSearchProducts([FromBody] Product name) 
    {
        // Fazemos uma pequena validação
        if (name.Name is null) return BadRequest("Envie um nome válido");

        // Procuramos com o método "startsWith" o produto
        // Transformo a primeira letra em maiúscula para a pesquisa
        var products = from b in _con.Products
                       where b.Name!.StartsWith(
                        char.ToUpper(name.Name[0]) + name.Name.Substring(1))
                        select b;

        // Outra validação
        if (products is null) return NotFound("Nenhum produto encontrado com este nome");

        // Log
        Log.Information($"Uma pesquisa: {name.Name}, foi feita");

        return Ok(products);
    }
    
    /*
    Neste método fazemos a inserção de um novo produto no banco de dados, sempre com
    validações sendo feitas.
    */
    [HttpPost]
    // Essa propriedade protege por autenticação o endpoint
    [Authorize]
    public async Task<ActionResult> PostProduct([FromBody] Product product)
    {
        if (product is null) return BadRequest("Produto inválido para cadastro");

        // Validação de primeira letra maiscula
        product.Name = char.ToUpper(product.Name[0]) + product.Name.Substring(1);

        _con.Products.Add(product);
        await _con.SaveChangesAsync();

        // Log
        Log.Information($"Um produto de nome: {product.Name} foi criado.");

        // Aqui envio a rota para ver o GET deste produto e o próprio produto registrado
        return Created($"/products/{product.Id}", product);
    }

    /*
    Método de Atualizar um produto, com validação e procura por ProductID
    */
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> PutProduct([FromRoute] long id, [FromBody] Product product)
    {
        // Procuramos e fazemos uma validação.
        var find = await _con.Products.FindAsync(Convert.ToInt32(id));
        if (find is null) return NotFound("Não encontramos nenhum produto com esse id");
        
        if (product is null) return BadRequest("Faça uma atualização válida");

        // Modificação
        find.Name = product.Name;
        find.Price = product.Price;

        _con.Products.Update(find);
        await _con.SaveChangesAsync();

        // Log
        Log.Information($"Um produto de nome: {product.Name} foi atualizado.");

        return Ok(find);
    } 

    /*
    Método Delete
    Buscamos um produto por id e o deletamos do banco de dados.
    */
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteProduct([FromRoute] long id)
    {
        // Procura e validação
        var product = await _con.Products.FindAsync(Convert.ToInt32(id));
        if (product is null) return NotFound("Produto não encontrado");

        // Log
        Log.Information($"Um produto de nome: {product.Name} foi deletado.");

        // Deletando produto
        _con.Products.Remove(product);
        await _con.SaveChangesAsync();

        return Ok("Produto deletado com sucesso");
    }

}