using Accounts.Domain;
using Accounts.Domain.Exceptions;
using MediatR;

namespace Accounts.Application.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler(IAccountRepository repository)
    : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AccountNotFoundException(request.Id);

        return account.ToDto();
    }
}
