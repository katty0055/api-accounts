using Accounts.Domain.Entities;
using Accounts.Domain.Interfaces;

namespace Accounts.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    // Lista simulada en memoria mientras conectas DbContext
    private static readonly List<Account> _accounts = new();

    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_accounts.AsReadOnly());
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        _accounts.Add(account);
        await Task.CompletedTask;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(true);
    }
}