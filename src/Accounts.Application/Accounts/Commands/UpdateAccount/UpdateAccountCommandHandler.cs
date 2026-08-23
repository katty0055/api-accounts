using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(
    IAccountRepository repository,
    ILogger<UpdateAccountCommandHandler> logger)
    : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Actualizando cuenta {AccountId}", request.Id);
        logger.LogDebug("Datos recibidos para actualizar cuenta {AccountId}: {OwnerName}, saldo {Balance}, activa {IsActive}",
            request.Id, request.OwnerName, request.Balance, request.IsActive);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account is null)
        {
            logger.LogWarning("No se encontró la cuenta {AccountId} para actualizar", request.Id);
            throw new AccountNotFoundException(request.Id);
        }

        account.Update(request.OwnerName, request.Balance, request.IsActive);

        repository.Update(account);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cuenta {AccountId} actualizada. Nuevo saldo: {Balance}, Activa: {IsActive}",
            account.Id, account.Balance, account.IsActive);

        return account.ToDto();
    }
}
