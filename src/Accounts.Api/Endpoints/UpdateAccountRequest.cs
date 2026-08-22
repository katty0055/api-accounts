namespace Accounts.Api.Endpoints;

/// <summary>
/// Datos de entrada para actualizar una cuenta existente.
/// </summary>
/// <param name="OwnerName">Nombre del titular de la cuenta.</param>
/// <param name="Balance">Saldo actualizado de la cuenta.</param>
/// <param name="IsActive">Indica si la cuenta se encuentra activa.</param>
public record UpdateAccountRequest(string OwnerName, decimal Balance, bool IsActive);
