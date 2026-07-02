using BettyWalletApp.Domain.Models;
using BettyWalletApp.Ports.Out;

namespace BettyWalletApp.Adapters.Out.Domain.Services
{
    public class CryptoRngAdapter : IRngEngine
    {
        public GameResult GenerateSpinResult(decimal betAmount)
        {
            double roll = Random.Shared.NextDouble() * 100;

            if (roll < 50)
            {
                return new GameResult(IsWin: false, Multiplier: 0, WinAmount: 0);
            }
            else if (roll < 90)
            {
                decimal multiplier = (decimal)(Random.Shared.NextDouble() * 2.0);
                return new GameResult(IsWin: true, multiplier, Math.Round(betAmount * multiplier, 2));
            }
            else
            {
                decimal multiplier = (decimal)(2.0 + (Random.Shared.NextDouble() * 8.0));
                return new GameResult(IsWin: true, multiplier, Math.Round(betAmount * multiplier, 2));
            }
        }
    }
}
