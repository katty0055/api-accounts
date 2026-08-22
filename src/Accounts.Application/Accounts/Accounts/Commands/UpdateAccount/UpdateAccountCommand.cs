using MediatR;

namespace Accounts.Application.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand(
    Guid Id,
    string OwnerName,
    decimal Balance,
    bool IsActive
) : IRequest<AccountDto>;
