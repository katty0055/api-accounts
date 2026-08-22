FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Accounts.Domain/*.csproj src/Accounts.Domain/
COPY src/Accounts.Application/*.csproj src/Accounts.Application/
COPY src/Accounts.Infrastructure/*.csproj src/Accounts.Infrastructure/
COPY src/Accounts.Api/*.csproj src/Accounts.Api/
RUN dotnet restore src/Accounts.Api/Accounts.Api.csproj

COPY src/ src/
RUN dotnet publish src/Accounts.Api/Accounts.Api.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Ejecutar como usuario no-root (incluido en las imágenes base de .NET)
USER app

ENTRYPOINT ["dotnet", "Accounts.Api.dll"]
