using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounts.Application.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(
    IAccountRepository repository,
    ILogger<DeleteAccountCommandHandler> logger)
    : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Eliminando cuenta {AccountId}", request.Id);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (account is null)
        {
            logger.LogWarning("No se encontró la cuenta {AccountId} para eliminar", request.Id);
            throw new AccountNotFoundException(request.Id);
        }

        repository.Delete(account);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Cuenta {AccountId} ({AccountNumber}) eliminada", account.Id, account.AccountNumber);
    }
}
