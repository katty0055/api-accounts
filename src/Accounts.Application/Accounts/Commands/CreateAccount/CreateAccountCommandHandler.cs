using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(
    IAccountRepository repository,
    ILogger<CreateAccountCommandHandler> logger)
    : IRequestHandler<CreateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creando cuenta {AccountNumber} para {OwnerName}", request.AccountNumber, request.OwnerName);
        logger.LogDebug("Verificando si ya existe una cuenta con número {AccountNumber}", request.AccountNumber);

        if (await repository.ExistsByAccountNumberAsync(request.AccountNumber, cancellationToken))
        {
            logger.LogWarning("Intento de crear cuenta duplicada con número {AccountNumber}", request.AccountNumber);
            throw new DuplicateAccountNumberException(request.AccountNumber);
        }

        var account = Account.Create(request.AccountNumber, request.OwnerName, request.InitialBalance);

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cuenta {AccountId} creada con número {AccountNumber} y saldo inicial {InitialBalance}",
            account.Id, account.AccountNumber, account.Balance);

        return account.ToDto();
    }
}