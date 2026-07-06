namespace BettyWalletApp.Adapters.Out
{
    public class GameSettings
    {
        public string EngineType { get; set; } = "Standard";
        public decimal MinBet { get; set; } = 1m;
        public decimal MaxBet { get; set; } = 10m;
        public int LoseProbability { get; set; } = 50;
        public int WinX2Probability { get; set; } = 40;
        public int WinX2ToX10Probability { get; set; } = 10;
    }
}
