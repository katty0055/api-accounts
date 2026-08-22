using MediatR;

namespace Accounts.Application.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto>;
