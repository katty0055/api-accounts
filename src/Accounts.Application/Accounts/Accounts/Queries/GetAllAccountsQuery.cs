using MediatR;

namespace Accounts.Application.Accounts.Queries.GetAllAccounts;

public record GetAllAccountsQuery : IRequest<IReadOnlyList<AccountDto>>;