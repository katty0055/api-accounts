using Accounts.Application.Accounts.Commands.CreateAccount;

namespace Accounts.Application.Tests;

public class CreateAccountCommandValidatorTests
{
    private readonly CreateAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NoDevuelveErrores()
    {
        var command = new CreateAccountCommand("0001-2345", "Juan Pérez", 100m);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_AccountNumberVacio_DevuelveError(string accountNumber)
    {
        var command = new CreateAccountCommand(accountNumber, "Juan Pérez", 100m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.AccountNumber));
    }

    [Fact]
    public void Validate_OwnerNameVacio_DevuelveError()
    {
        var command = new CreateAccountCommand("0001-2345", "", 100m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.OwnerName));
    }

    [Fact]
    public void Validate_SaldoInicialNegativo_DevuelveError()
    {
        var command = new CreateAccountCommand("0001-2345", "Juan Pérez", -1m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.InitialBalance));
    }
}
