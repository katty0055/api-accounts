using MediatR;

namespace Accounts.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IReadOnlyList<AccountDto>>
{
    public async Task<IReadOnlyList<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        // Simulamos una lista en memoria mientras configuramos la BD en Infraestructura
        var accounts = new List<AccountDto>
        {
            new(Guid.NewGuid(), "ACC-1001", "Katty", 1500.00m, true),
            new(Guid.NewGuid(), "ACC-1002", "Alex", 2300.50m, true)
        };

        return await Task.FromResult(accounts);
    }
}