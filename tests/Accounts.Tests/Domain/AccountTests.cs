using Accounts.Domain;
using Xunit;

namespace Accounts.Tests.Domain;

public class AccountTests
{
    [Fact]
    public void Create_DeberiaInicializarLaCuentaActivaConLosDatosIndicados()
    {
        var account = Account.Create("0001-2345", "Juan Pérez", 1500.50m);

        Assert.Equal("0001-2345", account.AccountNumber);
        Assert.Equal("Juan Pérez", account.OwnerName);
        Assert.Equal(1500.50m, account.Balance);
        Assert.True(account.IsActive);
        Assert.NotEqual(Guid.Empty, account.Id);
    }

    [Fact]
    public void Update_DeberiaModificarTitularSaldoYEstado()
    {
        var account = Account.Create("0001-2345", "Juan Pérez", 100m);

        account.Update("María Gómez", 250m, false);

        Assert.Equal("María Gómez", account.OwnerName);
        Assert.Equal(250m, account.Balance);
        Assert.False(account.IsActive);
    }
}
