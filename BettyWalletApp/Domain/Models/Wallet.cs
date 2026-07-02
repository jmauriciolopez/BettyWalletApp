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
                throw new InvalidOperationException("Saldo insuficiente para retirar fondos.");
            Balance -= amount;
        }

        public void ProcessBetAndWin(decimal betAmount, decimal winAmount)
        {
            ValidatePositiveAmount(betAmount);
            if (winAmount < 0) throw new ArgumentException("El premio no puede ser negativo.");
            if (Balance < betAmount)
                throw new InvalidOperationException("Saldo insuficiente para realizar la apuesta.");

            Balance = Balance - betAmount + winAmount;
        }

        private static void ValidatePositiveAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("El monto de la operación debe ser un número positivo.");
        }
    }
}
