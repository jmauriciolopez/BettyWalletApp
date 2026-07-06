using BettyWalletApp.Adapters.Out;

namespace BettyWalletApp.Tests;

public class AdapterTests
{
    [Fact]
    public void BucketRngAdapter_ShouldReturnLoss_WhenConfiguredForLosses()
    {
        var adapter = new BucketRngAdapter(loseProbability: 100, WinX2Probability: 0);

        var result = adapter.GenerateSpinResult(5m);

        Assert.False(result.IsWin);
        Assert.Equal(0m, result.Multiplier);
        Assert.Equal(0m, result.WinAmount);
    }

    [Fact]
    public void BucketRngAdapter_ShouldReturnWinWithSmallMultiplier_WhenConfiguredForX2Wins()
    {
        var adapter = new BucketRngAdapter(loseProbability: 0, WinX2Probability: 100);

        var result = adapter.GenerateSpinResult(5m);

        Assert.True(result.IsWin);
        Assert.Contains(result.Multiplier, new[] { 1m, 2m });
        Assert.Equal(Math.Round(5m * result.Multiplier, 2), result.WinAmount);
    }

    [Fact]
    public void CryptoRngAdapter_ShouldReturnLoss_WhenConfiguredForLosses()
    {
        var adapter = new CryptoRngAdapter(loseProbability: 100, WinX2Probability: 0);

        var result = adapter.GenerateSpinResult(5m);

        Assert.False(result.IsWin);
        Assert.Equal(0m, result.Multiplier);
        Assert.Equal(0m, result.WinAmount);
    }

    [Fact]
    public void CryptoRngAdapter_ShouldReturnHighMultiplierWin_WhenConfiguredForHighWins()
    {
        var adapter = new CryptoRngAdapter(loseProbability: 0, WinX2Probability: 0);

        var result = adapter.GenerateSpinResult(5m);

        Assert.True(result.IsWin);
        Assert.InRange(result.Multiplier, 2m, 10m);
        Assert.Equal(Math.Round(5m * result.Multiplier, 2), result.WinAmount);
    }
}
