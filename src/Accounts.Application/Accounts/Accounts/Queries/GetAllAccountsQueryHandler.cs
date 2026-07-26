using Accounts.Domain.Interfaces;
using MediatR;

namespace Accounts.Application.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IReadOnlyList<AccountDto>>
{
    private readonly IAccountRepository _repository;

    public GetAllAccountsQueryHandler(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _repository.GetAllAsync(cancellationToken);

        // Mapear entidades a DTOs
        return accounts.Select(a => new AccountDto(a.Id, a.AccountNumber, a.OwnerName, a.Balance, a.IsActive)).ToList();
    }
}