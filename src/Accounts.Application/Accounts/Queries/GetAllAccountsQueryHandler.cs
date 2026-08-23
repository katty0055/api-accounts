using Accounts.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler(
    IAccountRepository repository,
    ILogger<GetAllAccountsQueryHandler> logger)
    : IRequestHandler<GetAllAccountsQuery, IReadOnlyList<AccountDto>>
{
    public async Task<IReadOnlyList<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Iniciando consulta de todas las cuentas");

        var accounts = await repository.GetAllAsync(cancellationToken);
        logger.LogInformation("Se consultaron {AccountCount} cuentas", accounts.Count);
        return accounts.Select(a => a.ToDto()).ToList();
    }
}