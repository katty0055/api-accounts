using Accounts.Domain.Entities;
namespace Accounts.Domain.Interfaces;

public interface IAccountRepository
{
    Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Account account, CancellationToken cancellationToken = default);
}