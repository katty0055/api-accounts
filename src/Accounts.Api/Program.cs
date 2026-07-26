using Accounts.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS (Antes de builder.Build)
builder.Services.AddOpenApi();

// Registrar MediatR desde el ensamblado de Application
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Accounts.Application.Accounts.AccountDto).Assembly));

var app = builder.Build();

// 2. CONFIGURACIÓN DEL PIPELINE HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3. MAPEO DE ENDPOINTS
app.MapAccountEndpoints();

app.Run();