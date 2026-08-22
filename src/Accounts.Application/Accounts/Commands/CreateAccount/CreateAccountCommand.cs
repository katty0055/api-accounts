using MediatR;

namespace Accounts.Application.Accounts.Commands.CreateAccount;

/// <summary>
/// Comando para crear una nueva cuenta.
/// </summary>
/// <param name="AccountNumber">Número de cuenta único.</param>
/// <param name="OwnerName">Nombre del titular de la cuenta.</param>
/// <param name="InitialBalance">Saldo inicial de la cuenta.</param>
public record CreateAccountCommand(
    string AccountNumber,
    string OwnerName,
    decimal InitialBalance
) : IRequest<AccountDto>;