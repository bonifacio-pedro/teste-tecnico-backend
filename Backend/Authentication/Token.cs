namespace Backend.Authentication;

public class Token
{
    /*
    Classe criada para facilitar o código e continuar as boas práticas na utilização de autenticação JWT
    */
    public string? TokenAuth { get; set; }
    public int UserId { get; set; }
}