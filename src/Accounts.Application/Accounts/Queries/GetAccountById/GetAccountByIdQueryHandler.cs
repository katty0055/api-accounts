using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler(
    IAccountRepository repository,
    ILogger<GetAccountByIdQueryHandler> logger)
    : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Consultando cuenta {AccountId}", request.Id);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account is null)
        {
            logger.LogWarning("No se encontró la cuenta {AccountId}", request.Id);
            throw new AccountNotFoundException(request.Id);
        }

        return account.ToDto();
    }
}
