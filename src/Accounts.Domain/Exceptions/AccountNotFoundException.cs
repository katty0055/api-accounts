namespace Accounts.Domain.Exceptions;

public class AccountNotFoundException(Guid accountId)
    : DomainException($"No se encontró la cuenta con id '{accountId}'.");
