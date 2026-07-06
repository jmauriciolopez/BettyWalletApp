using BettyWalletApp.Domain.Models;
using BettyWalletApp.Ports.Out;

namespace BettyWalletApp.Adapters.Out
{

    public class BucketRngAdapter : IRngEngine
    {
        private readonly List<PreCalculatedOutcome> _bucket = new(1000);
        private readonly object _bucketLock = new();
        private readonly int _loseProbability;
        private readonly int _winX2Probability;
        public BucketRngAdapter(int loseProbability, int WinX2Probability)
        {
            _loseProbability = loseProbability;
            _winX2Probability = WinX2Probability;
            InitializeBucket();
        }

        public GameResult GenerateSpinResult(decimal betAmount)
        {
            PreCalculatedOutcome outcome;

            lock (_bucketLock)
            {
                if (_bucket.Count == 0)
                {
                    InitializeBucket();
                }

                int lastIndex = _bucket.Count - 1;
                outcome = _bucket[lastIndex];
                _bucket.RemoveAt(lastIndex);
            }

            decimal winAmount = Math.Round(betAmount * outcome.Multiplier, 2);
            return new GameResult(outcome.IsWin, outcome.Multiplier, winAmount);
        }

        private void InitializeBucket()
        {
            var tempPool = new List<PreCalculatedOutcome>(1000);

            for (int i = 0; i < _loseProbability*10 ; i++) tempPool.Add(new PreCalculatedOutcome(false, 0));
            for (int i = 0; i < _winX2Probability*10; i++) tempPool.Add(new PreCalculatedOutcome(true, Random.Shared.Next(1, 3)));
            for (int i = 0; i < 1000- _loseProbability * 10- _winX2Probability * 10; i++) tempPool.Add(new PreCalculatedOutcome(true, Random.Shared.Next(2, 11)));

            int n = tempPool.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Shared.Next(n + 1);
                (tempPool[k], tempPool[n]) = (tempPool[n], tempPool[k]);
            }

            _bucket.Clear();
            _bucket.AddRange(tempPool);
        }

        private record PreCalculatedOutcome(bool IsWin, int Multiplier);
    }
}
