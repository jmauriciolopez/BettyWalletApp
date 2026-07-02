using BettyWalletApp.Domain.Models;

namespace BettyWalletApp.Ports.Out
{
    public interface IRngEngine
    {
        GameResult GenerateSpinResult(decimal betAmount);
    }
}
