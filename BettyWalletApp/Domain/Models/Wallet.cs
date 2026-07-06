namespace BettyWalletApp.Domain.Models
{
    public class Wallet
    {
        public decimal Balance { get; private set; } = 0.00m;

        public void Deposit(decimal amount)
        {
            ValidatePositiveAmount(amount);
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            ValidatePositiveAmount(amount);
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds to withdraw.");
            Balance -= amount;
        }

        public void ProcessBetAndWin(decimal betAmount, decimal winAmount)
        {
            ValidatePositiveAmount(betAmount);
            if (winAmount < 0) throw new ArgumentException("The prize cannot be negative.");
            if (Balance < betAmount)
                throw new InvalidOperationException("Insufficient funds to withdraw.");

            Balance = Balance - betAmount + winAmount;
        }

        private static void ValidatePositiveAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The transaction amount must be a positive number.");
        }
    }
}
