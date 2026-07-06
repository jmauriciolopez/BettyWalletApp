using BettyWalletApp.Domain.Models;
using BettyWalletApp.Domain.Services;
using BettyWalletApp.Ports.Out;

namespace BettyWalletApp.Tests;

public class WalletApplicationServiceTests
{
    [Fact]
    public void Deposit_ShouldIncreaseBalance()
    {
        var service = CreateService();

        var result = service.Deposit(15m);

        Assert.Equal(15m, result);
        Assert.Equal(15m, service.GetBalance());
    }

    [Fact]
    public void Play_ShouldUpdateBalance_WhenBetIsValid()
    {
        var rng = new StubRngEngine(new GameResult(true, 1.5m, 7.5m));
        var service = CreateService(rng);
        service.Deposit(20m);

        var result = service.Play(5m);

        Assert.Equal(7.5m, result.WinAmount);
        Assert.Equal(22.5m, service.GetBalance());
    }

    [Fact]
    public void Play_ShouldThrow_WhenBetIsOutsideConfiguredRange()
    {
        var service = CreateService();

        Assert.Throws<ArgumentOutOfRangeException>(() => service.Play(0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => service.Play(100m));
    }

    [Fact]
    public void Play_ShouldThrow_WhenBalanceIsInsufficient()
    {
        var service = CreateService();

        Assert.Throws<InvalidOperationException>(() => service.Play(5m));
    }

    private static WalletApplicationService CreateService(IRngEngine? rngEngine = null)
    {
        rngEngine ??= new StubRngEngine(new GameResult(false, 0m, 0m));
        var repo = new TestWalletRepository();
        return new WalletApplicationService(rngEngine, repo, minBet: 1m, maxBet: 10m);
    }

    private sealed class StubRngEngine(GameResult result) : IRngEngine
    {
        private readonly GameResult _result = result;

        public GameResult GenerateSpinResult(decimal betAmount) => _result;
    }
    private sealed class TestWalletRepository : IWalletRepository
    {
        private readonly Wallet _wallet = new();
        public Wallet Get() => _wallet;
        public void Save(Wallet wallet)
        {
        }
    }
}
