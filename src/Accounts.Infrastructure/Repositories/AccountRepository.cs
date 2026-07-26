using Accounts.Domain.Entities;
using Accounts.Domain.Interfaces;
// using Accounts.Infrastructure.Persistence; // Tu DbContext de EF Core irá aquí

namespace Accounts.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    // Inyectarás tu ApplicationDbContext cuando configuremos EF Core

    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Lógica de consulta a la BD (ej. await _context.Accounts.ToListAsync(cancellationToken))
        return new List<Account>();
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return null;
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        // await _context.Accounts.AddAsync(account, cancellationToken);
    }
}