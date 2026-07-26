namespace Accounts.Infrastructure.Repositories;
using Accounts.Domain;

public class AccountRepository : IAccountRepository
{
    // ES OBLIGATORIO QUE SEA 'static' para mantener los datos en memoria mientras corre la app
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