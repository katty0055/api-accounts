using Accounts.Application.Accounts.Commands.CreateAccount;
using Xunit;

namespace Accounts.Tests.Application;

public class CreateAccountCommandValidatorTests
{
    private readonly CreateAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_ComandoValido_NoDeberiaTenerErrores()
    {
        var command = new CreateAccountCommand("0001-2345", "Juan Pérez", 100m);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_AccountNumberVacio_DeberiaFallar(string accountNumber)
    {
        var command = new CreateAccountCommand(accountNumber, "Juan Pérez", 100m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.AccountNumber));
    }

    [Fact]
    public void Validate_OwnerNameVacio_DeberiaFallar()
    {
        var command = new CreateAccountCommand("0001-2345", "", 100m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.OwnerName));
    }

    [Fact]
    public void Validate_SaldoInicialNegativo_DeberiaFallar()
    {
        var command = new CreateAccountCommand("0001-2345", "Juan Pérez", -1m);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateAccountCommand.InitialBalance));
    }
}
