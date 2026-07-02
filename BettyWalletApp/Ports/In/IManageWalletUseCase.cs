namespace BettyWalletApp.Ports.In
{
    public interface IManageWalletUseCase
    {
        decimal GetBalance();
        decimal Deposit(decimal amount);
        decimal Withdraw(decimal amount);
    }
}
