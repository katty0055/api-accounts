using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;

namespace Accounts.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(IAccountRepository repository)
    : IRequestHandler<CreateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsByAccountNumberAsync(request.AccountNumber, cancellationToken))
        {
            throw new DuplicateAccountNumberException(request.AccountNumber);
        }

        var account = Account.Create(request.AccountNumber, request.OwnerName, request.InitialBalance);

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return account.ToDto();
    }
}