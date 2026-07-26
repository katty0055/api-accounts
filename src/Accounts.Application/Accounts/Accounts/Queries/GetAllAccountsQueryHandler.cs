using Accounts.Domain;
using MediatR;

namespace Accounts.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler(IAccountRepository repository)
    : IRequestHandler<GetAllAccountsQuery, IReadOnlyList<AccountDto>>
{
    public async Task<IReadOnlyList<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await repository.GetAllAsync(cancellationToken);
        return accounts.Select(a => a.ToDto()).ToList();
    }
}