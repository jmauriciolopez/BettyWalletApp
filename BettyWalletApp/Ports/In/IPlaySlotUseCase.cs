using BettyWalletApp.Domain.Models;

namespace BettyWalletApp.Ports.In
{
    public interface IPlaySlotUseCase
    {
        GameResult Play(decimal betAmount);
    }
}
