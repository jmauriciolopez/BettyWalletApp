using BettyWalletApp.Domain.Models;

namespace BettyWalletApp.Ports.Out
{
    public interface IWalletRepository
    {
        Wallet Get();
        void Save(Wallet wallet);
    }
}
