using BettyWalletApp.Domain.Models;
using BettyWalletApp.Ports.Out;

namespace BettyWalletApp.Adapters.Out
{
    public class CryptoRngAdapter : IRngEngine
    {
        private readonly int _loseProbability;
        private readonly int _winX2Probability;
        public CryptoRngAdapter(int loseProbability , int WinX2Probability   )
        {
            _loseProbability = loseProbability;
            _winX2Probability = WinX2Probability;
        }
        public GameResult GenerateSpinResult(decimal betAmount)
        {
            double roll = Random.Shared.NextDouble() * 100;

            if (roll < _loseProbability)
            {
                return new GameResult(IsWin: false, Multiplier: 0, WinAmount: 0);
            }
            else if (roll < _loseProbability + _winX2Probability)
            {
                int multiplier = Random.Shared.Next(1, 3);
                return new GameResult(IsWin: true, multiplier, Math.Round(betAmount * multiplier, 2));
            }
            else
            {
                int multiplier = Random.Shared.Next(2, 11);
                return new GameResult(IsWin: true, multiplier, Math.Round(betAmount * multiplier, 2));
            }
        }
    }
}
