using MediatR;

namespace Accounts.Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(
    string AccountNumber,
    string OwnerName,
    decimal InitialBalance
) : IRequest<AccountDto>;