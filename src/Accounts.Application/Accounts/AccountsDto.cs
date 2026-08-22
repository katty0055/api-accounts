namespace Accounts.Application.Accounts;

/// <summary>
/// Representa los datos de una cuenta expuestos por la API.
/// </summary>
/// <param name="Id">Identificador único de la cuenta.</param>
/// <param name="AccountNumber">Número de cuenta.</param>
/// <param name="OwnerName">Nombre del titular de la cuenta.</param>
/// <param name="Balance">Saldo actual de la cuenta.</param>
/// <param name="IsActive">Indica si la cuenta se encuentra activa.</param>
public record AccountDto(
    Guid Id,
    string AccountNumber,
    string OwnerName,
    decimal Balance,
    bool IsActive
);