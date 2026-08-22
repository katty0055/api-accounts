using MediatR;

namespace Accounts.Application.Accounts.Commands.UpdateAccount;

/// <summary>
/// Comando para actualizar una cuenta existente.
/// </summary>
/// <param name="Id">Identificador de la cuenta a actualizar.</param>
/// <param name="OwnerName">Nuevo nombre del titular.</param>
/// <param name="Balance">Nuevo saldo de la cuenta.</param>
/// <param name="IsActive">Nuevo estado de actividad de la cuenta.</param>
public record UpdateAccountCommand(
    Guid Id,
    string OwnerName,
    decimal Balance,
    bool IsActive
) : IRequest<AccountDto>;
