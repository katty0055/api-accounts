using Accounts.Application.Accounts;
using Accounts.Application.Accounts.Commands.CreateAccount;
using Accounts.Application.Accounts.Queries.GetAllAccounts;
using MediatR;

namespace Accounts.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/minimal/accounts").WithTags("Accounts (Minimal API)");

        // GET: Mostrar todas las cuentas
        group.MapGet("", async (ISender sender, CancellationToken cancellationToken) =>
            Results.Ok(await sender.Send(new GetAllAccountsQuery(), cancellationToken)))
            .Produces<IReadOnlyList<AccountDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        // POST: Crear una nueva cuenta
        group.MapPost("", async (CreateAccountCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var account = await sender.Send(command, cancellationToken);
            return Results.Created($"api/minimal/accounts/{account.Id}", account);
        })
            .Produces<AccountDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }
}
