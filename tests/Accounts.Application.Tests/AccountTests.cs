using Accounts.Domain;

namespace Accounts.Application.Tests;

public class AccountTests
{
    [Fact]
    public void Create_AsignaValoresYQuedaActiva()
    {
        var account = Account.Create("0001-2345", "Juan Pérez", 100m);

        Assert.Equal("0001-2345", account.AccountNumber);
        Assert.Equal("Juan Pérez", account.OwnerName);
        Assert.Equal(100m, account.Balance);
        Assert.True(account.IsActive);
        Assert.NotEqual(Guid.Empty, account.Id);
    }

    [Fact]
    public void Update_ModificaLosDatos()
    {
        var account = Account.Create("0001-2345", "Juan Pérez", 100m);

        account.Update("María López", 250m, false);

        Assert.Equal("María López", account.OwnerName);
        Assert.Equal(250m, account.Balance);
        Assert.False(account.IsActive);
    }
}
