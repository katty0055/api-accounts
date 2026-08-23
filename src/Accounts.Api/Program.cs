using Accounts.Api.Endpoints;
using Accounts.Api.ErrorHandling;
using Accounts.Api.Middleware;
using Accounts.Application.Accounts.Commands.CreateAccount;
using Accounts.Application.Common.Behaviors;
using Accounts.Domain;
using Accounts.Infrastructure.Persistence;
using Accounts.Infrastructure.Repositories;
using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging estructurado con Serilog (consola + Seq para centralización)
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    var seqUrl = context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341";

    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "Accounts.Api")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .Enrich.WithProperty("PodName", Environment.MachineName)
        .WriteTo.Console()
        .WriteTo.Seq(seqUrl);
});

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
{
    cfg.RegisterServicesFromAssembly(typeof(Accounts.Application.Accounts.AccountDto).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// Registrar validadores de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountCommandValidator>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// Versionado de la API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Manejo centralizado de errores -> ProblemDetails
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Aplicar migraciones pendientes al iniciar (necesario para contenedores con BD nueva)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// 2. CONFIGURACIÓN DEL PIPELINE HTTP
app.UseSerilogRequestLogging();

app.UseExceptionHandler(_ => { });

// Logging estructurado por request: Information (éxito), Warning (validación) y Error (excepciones).
// Va después de UseExceptionHandler para poder observar las excepciones antes de que se conviertan en ProblemDetails.
app.UseRequestLogging();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

// 3. MAPEO DE ENDPOINTS
app.MapAccountEndpoints();

app.Run();