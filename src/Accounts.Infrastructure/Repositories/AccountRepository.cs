using Accounts.Domain;
using Accounts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Repositories;

public class AccountRepository(ApplicationDbContext context) : IAccountRepository
{
    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Consulta directa a la tabla de PostgreSQL
        return await context.Accounts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        // Prepara la inserción en el DbContext
        await context.Accounts.AddAsync(account, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Confirma los cambios y ejecuta el INSERT en PostgreSQL
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}