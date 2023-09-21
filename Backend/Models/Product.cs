using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Product
{
    /*
    Classe utilizada para o método CODE-FIRST, onde escrevemos a tabela e os atributos do
    banco antes da migração para utilizar durante o código.
    Essa é a entidade produto da aplicação.

    Coloquei também data annotations para melhorar o código, o deixando mais visível, limpo
    e optimizado
    */
    public int Id { get; set; }
    
    [Required]
    [StringLength(300)]
    public string ? Name { get; set; }

    [Required]
    [Column(TypeName = "decimal(5, 2)")]
    public decimal ? Price { get; set; }
}