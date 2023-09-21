using Backend.Context;
using Backend.Models;
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
    public ProductsController(AppDbContext context)
    {
        _con = context;
    }
    

    /*
    Aqui fiz o método GET, retornando um IEnumerable (uma lista em JSON) e um ActionResult,
    que é um código HTTP, inclusive utilizando programação assíncrona para melhor fluídez.
    E fiz algumas verifacações.
    */
    [HttpGet]
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
    public async Task<ActionResult<Product>> GetOneProduct([FromRoute] long id)
    {
        // Procuramos e fazemos uma validação.
        var product = await _con.Products.FindAsync(Convert.ToInt32(id));

        if (product is null) return NotFound("Nenhum produto encontrado");

        // Log
        Log.Information($"Um produto de nome: {product.Name} foi buscado.");

        return Ok(product);
    }

    
    /*
    Neste método fazemos a inserção de um novo produto no banco de dados, sempre com
    validações sendo feitas.
    */
    [HttpPost]
    public async Task<ActionResult> PostProduct([FromBody] Product product)
    {
        if (product is null) return BadRequest("Produto inválido para cadastro");

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