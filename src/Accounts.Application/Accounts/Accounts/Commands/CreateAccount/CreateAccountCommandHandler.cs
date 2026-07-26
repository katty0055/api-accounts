using MediatR;

namespace Accounts.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // Lógica simulada de creación de entidad/guardado
        var newAccount = new AccountDto(
            Guid.NewGuid(),
            request.AccountNumber,
            request.OwnerName,
            request.InitialBalance,
            true
        );

        return await Task.FromResult(newAccount);
    }
}