namespace Accounts.Domain;

public class Account
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string AccountNumber { get; private set; } = string.Empty;
    public string OwnerName { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public bool IsActive { get; private set; } = true;

    public static Account Create(string accountNumber, string ownerName, decimal initialBalance)
    {
        return new Account
        {
            AccountNumber = accountNumber,
            OwnerName = ownerName,
            Balance = initialBalance,
            IsActive = true
        };
    }

    public void Update(string ownerName, decimal balance, bool isActive)
    {
        OwnerName = ownerName;
        Balance = balance;
        IsActive = isActive;
    }
}