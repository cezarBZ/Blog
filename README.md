# Blog

**Descrição** Este é um projeto de blog desenvolvido em **C#** utilizando **.NET 8**. Ele segue uma estrutura modular com separação em camadas para facilitar a escalabilidade e manutenção do código.

## Tecnologias Utilizadas
* **Linguagem:** C#
* **Framework:** .NET 8
* **Containerização:** Docker
* **Arquitetura:** Clean Architecture
* **Banco de Dados:** SQL Server

## Estrutura do Projeto
O projeto está dividido em diferentes camadas:
* **Blog.API:** Contém os endpoints da API REST.
* **Blog.Application:** Camada de aplicação responsável pelas regras de negócio.
* **Blog.Domain:** Define as entidades e contratos da aplicação.
* **Blog.Infrastructure:** Implementação dos serviços de dados e integração.
* **Blog.Tests:** Conjunto de testes para garantir a qualidade do código.

## Instalação e Execução

### Pré-requisitos
Certifique-se de ter instalado:
* .NET 8 SDK
* Docker

### Passos
1. Clone o repositório:

```sh
git clone https://github.com/cezarBZ/Blog.git
cd Blog
```

2. Restaure as dependências:

```sh
dotnet restore
```

3. Compile a aplicação:

```sh
dotnet build
```

4. Execute o projeto:

```sh
dotnet run --project Blog.API
```

5. Para rodar via Docker:

```sh
docker build -t blog-app .
docker run -p 5000:80 blog-app
```
