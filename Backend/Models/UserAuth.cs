using System.ComponentModel.DataAnnotations;

namespace Backed.Models;

public class UserAuth 
{
    /*
    Modelo utilizado para criar usuários ao se autenticar no JWT
    */
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
}

