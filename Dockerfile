# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho
WORKDIR /Blog

# Copia os arquivos de projeto e restaura as dependências
COPY Blog.Domain/Blog.Domain.csproj Blog.Domain/
COPY Blog.Application/Blog.Application.csproj Blog.Application/
COPY Blog.Infrastructure/Blog.Infrastructure.csproj Blog.Infrastructure/
COPY Blog.API/Blog.API.csproj Blog.API/

# Restaura as dependências do projeto
RUN dotnet restore Blog.API/Blog.API.csproj

# Copia todo o código fonte para o contêiner
COPY . .

# Define o diretório de trabalho para a API
WORKDIR /Blog/Blog.API

# Compila o projeto e gera a saída publicada
RUN dotnet publish Blog.API.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Define o diretório de trabalho no contêiner
WORKDIR /app

# Copia os arquivos publicados da etapa anterior
COPY --from=build /app/publish .

# Define o comando de entrada
ENTRYPOINT ["dotnet", "Blog.API.dll"]

