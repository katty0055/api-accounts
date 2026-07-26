using Accounts.Domain;

namespace Accounts.Application.Accounts;

public static class AccountMappings
{
    public static AccountDto ToDto(this Account account)
    {
        return new AccountDto(
            account.Id,
            account.AccountNumber,
            account.OwnerName,
            account.Balance,
            account.IsActive
        );
    }
}
