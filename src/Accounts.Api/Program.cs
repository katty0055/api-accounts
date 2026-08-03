using Accounts.Api.Endpoints;
using Accounts.Domain;
using Accounts.Infrastructure.Persistence;
using Accounts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS (Antes de builder.Build)
builder.Services.AddOpenApi();

// Registrar DbContext con PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString)
           .UseSnakeCaseNamingConvention();
});

// Registrar MediatR desde el ensamblado de Application
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Accounts.Application.Accounts.AccountDto).Assembly));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();

// 2. CONFIGURACIÓN DEL PIPELINE HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 3. MAPEO DE ENDPOINTS
app.MapAccountEndpoints();

app.Run();