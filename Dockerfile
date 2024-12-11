#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blog.API/Blog.API.csproj", "Blog.API/"]
COPY ["Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["Blog.Infrastructure/Blog.Infrastructure.csproj", "Blog.Infrastructure/"]
RUN dotnet restore "./Blog.API/Blog.API.csproj"
COPY . .
WORKDIR "/src/Blog.API"
RUN dotnet build "./Blog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.API.dll"]