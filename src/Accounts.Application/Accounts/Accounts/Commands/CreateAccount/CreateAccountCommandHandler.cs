using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;

namespace Accounts.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(IAccountRepository repository)
    : IRequestHandler<CreateAccountCommand, AccountDto> // <-- Cambiado de Guid a AccountDto
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsByAccountNumberAsync(request.AccountNumber, cancellationToken))
        {
            throw new DuplicateAccountNumberException(request.AccountNumber);
        }

        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = request.AccountNumber,
            OwnerName = request.OwnerName,
            Balance = request.InitialBalance,
            IsActive = true
        };

        // 1. Agregar a la lista/BD
        await repository.AddAsync(account, cancellationToken);

        // 2. Guardar cambios
        await repository.SaveChangesAsync(cancellationToken);

        // 3. Mapear a DTO para retornar al endpoint
        return account.ToDto();
    }
}