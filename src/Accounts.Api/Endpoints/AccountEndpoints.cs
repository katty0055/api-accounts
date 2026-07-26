using Accounts.Application.Accounts.Commands.CreateAccount;
using Accounts.Application.Accounts.Queries.GetAllAccounts;
using MediatR;

namespace Accounts.Api.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/accounts");

        // GET: Obtener todas
        group.MapGet("/", async (ISender mediator) =>
        {
            var accounts = await mediator.Send(new GetAllAccountsQuery());
            return Results.Ok(accounts);
        });

        // POST: Crear cuenta
        group.MapPost("/", async (CreateAccountCommand command, ISender mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/minimal/accounts/{id}", id);
        });
    }
}
