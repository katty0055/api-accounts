using Accounts.Application.Accounts;
using Accounts.Application.Accounts.Commands.CreateAccount;
using Accounts.Application.Accounts.Commands.DeleteAccount;
using Accounts.Application.Accounts.Commands.UpdateAccount;
using Accounts.Application.Accounts.Queries.GetAccountById;
using Accounts.Application.Accounts.Queries.GetAllAccounts;
using Asp.Versioning;
using Asp.Versioning.Builder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace Accounts.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("api/v{version:apiVersion}/accounts")
            .WithApiVersionSet(apiVersionSet)
            .WithTags("Accounts");

        var accountExample = new AccountDto(
            Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            "0001-2345-6789",
            "Juan Pérez",
            1500.50m,
            true);

        // GET: Mostrar todas las cuentas
        group.MapGet("", async (ISender sender, CancellationToken cancellationToken) =>
            Results.Ok(await sender.Send(new GetAllAccountsQuery(), cancellationToken)))
            .WithName("GetAllAccounts")
            .WithSummary("Obtiene todas las cuentas")
            .WithDescription("Devuelve la lista completa de cuentas registradas en el sistema.")
            .Produces<IReadOnlyList<AccountDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation =>
            {
                operation.Responses["200"].Content["application/json"].Example =
                    new JsonArray(ToOpenApiExample(accountExample));
                return operation;
            })
            .MapToApiVersion(1.0);

        // GET /{id}: Obtener una cuenta por id
        group.MapGet("{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            Results.Ok(await sender.Send(new GetAccountByIdQuery(id), cancellationToken)))
            .WithName("GetAccountById")
            .WithSummary("Obtiene una cuenta por id")
            .WithDescription("Devuelve los datos de una cuenta específica. Si no existe, retorna 404.")
            .Produces<AccountDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation =>
            {
                operation.Responses["200"].Content["application/json"].Example = ToOpenApiExample(accountExample);
                return operation;
            })
            .MapToApiVersion(1.0);

        // POST: Crear una nueva cuenta
        group.MapPost("", async (CreateAccountCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var account = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/v1/accounts/{account.Id}", account);
        })
            .WithName("CreateAccount")
            .WithSummary("Crea una nueva cuenta")
            .WithDescription("Crea una cuenta nueva con un número de cuenta, un titular y un saldo inicial. Falla con 409 si el número de cuenta ya existe.")
            .Produces<AccountDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation =>
            {
                operation.Responses["201"].Content["application/json"].Example = ToOpenApiExample(accountExample);
                return operation;
            })
            .MapToApiVersion(1.0);

        // PUT /{id}: Actualizar una cuenta existente
        group.MapPut("{id:guid}", async (Guid id, UpdateAccountRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateAccountCommand(id, request.OwnerName, request.Balance, request.IsActive);
            var account = await sender.Send(command, cancellationToken);
            return Results.Ok(account);
        })
            .WithName("UpdateAccount")
            .WithSummary("Actualiza una cuenta existente")
            .WithDescription("Actualiza el titular, saldo y estado de una cuenta. Si no existe, retorna 404.")
            .Produces<AccountDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(operation =>
            {
                operation.Responses["200"].Content["application/json"].Example = ToOpenApiExample(accountExample);
                return operation;
            })
            .MapToApiVersion(1.0);

        // DELETE /{id}: Eliminar una cuenta
        group.MapDelete("{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new DeleteAccountCommand(id), cancellationToken);
            return Results.NoContent();
        })
            .WithName("DeleteAccount")
            .WithSummary("Elimina una cuenta")
            .WithDescription("Elimina una cuenta existente por id. Si no existe, retorna 404.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .MapToApiVersion(1.0);
    }

    private static JsonObject ToOpenApiExample(AccountDto account) => new()
    {
        ["id"] = account.Id.ToString(),
        ["accountNumber"] = account.AccountNumber,
        ["ownerName"] = account.OwnerName,
        ["balance"] = account.Balance,
        ["isActive"] = account.IsActive
    };
}
