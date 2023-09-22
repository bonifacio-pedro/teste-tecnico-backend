Aplicação feita para teste técnico com o tema: Vendas diversas de um brechó.

# Como rodar?
* Tenha o .NET 7 instalado em sua máquina;
* Clone o repositório
* Entre na pasta do projeto, abra uma linha de comando e rode:
```
dotnet run
```

## Importante

A porta padrão (utilizei localhost para o sistema), é http://localhost:5200 , caso na máquina em que o teste esteja sendo feito essa porta esteja em uso, pode-se modificar ela em Backend->Properties->launchSettings.json, imagem de exemplificação:

![image](https://github.com/bonifacio-pedro/teste-tecnico-backend/assets/123605656/2f31e1c0-ee84-4a8a-8210-787ae4ed0bc9)


# Tecnologias utilizadas:
* C# .NET
* ASP.NET
* RESTful API
* Entity Framework
* Serilog
* Bearer JWT Authetication
* Injestão de dependência
* SQLite

# Especificações e extras:
Endpoints feitos para produtos e login (autenticação JWT).
Endpoint feito para pesquisa.
Validações diversas.
Logging e autenticação.

# Estrutura das pastas:
* Authetication -> Modelo do token
* Context -> Contexto de conexão com o DB
* Models -> Modelos (tanto o de produtos como o de autenticação)
* Services -> Serviços gerais que são necessários
* Controllers -> Controladores dos endpoints
