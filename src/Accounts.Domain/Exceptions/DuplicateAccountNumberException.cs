namespace Accounts.Domain.Exceptions;

public class DuplicateAccountNumberException(string accountNumber)
    : DomainException($"Ya existe una cuenta con el número '{accountNumber}'.");
