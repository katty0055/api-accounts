using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;

namespace Accounts.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(IAccountRepository repository)
    : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AccountNotFoundException(request.Id);

        account.OwnerName = request.OwnerName;
        account.Balance = request.Balance;
        account.IsActive = request.IsActive;

        repository.Update(account);
        await repository.SaveChangesAsync(cancellationToken);

        return account.ToDto();
    }
}
