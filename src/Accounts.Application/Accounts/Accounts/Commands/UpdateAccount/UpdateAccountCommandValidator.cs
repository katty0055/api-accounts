using FluentValidation;

namespace Accounts.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.OwnerName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El saldo no puede ser negativo.");
    }
}
