Aplicação feita para teste técnico com o tema: Vendas diversas de um brechó.

# Como rodar?
* Tenha o .NET 7 instalado em sua máquina;
* Clone o repositório
* Entre na pasta do projeto, abra uma linha de comando e rode:
```
dotnet run
```

#Tecnologias utilizadas:
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
