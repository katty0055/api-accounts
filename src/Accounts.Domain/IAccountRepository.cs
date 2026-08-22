namespace Accounts.Domain;

public interface IAccountRepository
{
    Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
    void Update(Account account);
    void Delete(Account account);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}