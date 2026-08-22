using MediatR;

namespace Accounts.Application.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest;
