namespace Accounts.Application.Accounts;

public record AccountDto(
    Guid Id,
    string AccountNumber,
    string OwnerName,
    decimal Balance,
    bool IsActive
);