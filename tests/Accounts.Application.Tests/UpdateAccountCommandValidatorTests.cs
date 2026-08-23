using Accounts.Application.Accounts.Commands.UpdateAccount;

namespace Accounts.Application.Tests;

public class UpdateAccountCommandValidatorTests
{
    private readonly UpdateAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NoDevuelveErrores()
    {
        var command = new UpdateAccountCommand(Guid.NewGuid(), "María López", 500m, true);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_IdVacio_DevuelveError()
    {
        var command = new UpdateAccountCommand(Guid.Empty, "María López", 500m, true);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateAccountCommand.Id));
    }

    [Fact]
    public void Validate_BalanceNegativo_DevuelveError()
    {
        var command = new UpdateAccountCommand(Guid.NewGuid(), "María López", -10m, true);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateAccountCommand.Balance));
    }
}
