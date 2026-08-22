using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;

namespace Accounts.Application.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(IAccountRepository repository)
    : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AccountNotFoundException(request.Id);

        repository.Delete(account);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
