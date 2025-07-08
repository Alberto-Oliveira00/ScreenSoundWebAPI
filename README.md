# ScreenSound

Uma plataforma web para cadastro e gerenciamento de músicas, artistas e gênros musicais, desenvolvida com C#, .NET, Blazor e Entity Framework. Este projeto foi criado como forma de consolidar conhecimentos adquiridos durante a formação em C# e .NET na plataforma Alura.

## 🚀 Tecnologias utilizadas

* C#
* ASP.NET Core
* Entity Framework Core
* ASP.NET Web API
* Blazor WebAssembly
* Azure Blob Storage
* SQL Server
* Swagger

## 🎯 Funcionalidades

* Cadastro de músicas, artistas e gênros
* Upload de imagens para o Azure Blob Storage
* CRUD completo com Entity Framework
* Consumo de API no front-end com Blazor
* Interface responsiva e moderna
* Documentação da API com Swagger
* Publicação na nuvem com Azure

## 📚 Conceitos aplicados

* Programacão Orientada a Objetos (POO)
* Herança, encapsulamento, interfaces e polimorfismo
* Consumo de APIs externas e manipulação de JSON
* LINQ e coleções do .NET
* Tratamento de exceções
* Arquitetura em camadas
* Migrations e persistência com Entity Framework

## 🛠 Como rodar o projeto localmente

### Pré-requisitos

* [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) instalado
* SQL Server (ou outro banco compatível com Entity Framework Core)
* Conta na Azure (opcional, para testar upload em nuvem)

### Passos

1. Clone o repositório:

   ```bash
   git clone https://github.com/Alberto-Oliveira00/ScreenSoundWebAPI.git
   ```

2. Acesse o diretório da API:

   ```bash
   cd ScreenSoundWebAPI/ScreenSound.API
   ```

3. Aplique as migrations para criar o banco de dados:

   ```bash
   dotnet ef database update
   ```

4. Execute a aplicação:

   ```bash
   dotnet run
   ```

5. Acesse o Swagger para testar os endpoints:

   ```
   https://localhost:{porta}/swagger
   ```

> Para executar o front-end Blazor:
>
> * Navegue até o projeto Blazor correspondente
> * Execute via `dotnet run` ou abra no Visual Studio


## 🌐 Acesse o projeto online

* Repositório GitHub: [ScreenSound](https://github.com/Alberto-Oliveira00/ScreenSoundWebAPI)
* App publicado no Azure: [screensound-webapp-eygwgachadascdcg.brazilsouth-01.azurewebsites.net](https://screensound-webapp-eygwgachadascdcg.brazilsouth-01.azurewebsites.net/)

## 🙌 Autor

Desenvolvido por **Alberto Oliveira**
🔗 [LinkedIn](https://www.linkedin.com/in/alberto-oliveira00)

## 📝 Licença

Este projeto está licenciado sob a licença **MIT**.
Sinta-se à vontade para utilizar, modificar e compartilhar!
