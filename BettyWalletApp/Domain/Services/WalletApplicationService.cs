using BettyWalletApp.Domain.Models;
using BettyWalletApp.Ports.In;
using BettyWalletApp.Ports.Out;

namespace BettyWallet.Domain.Services;

public class WalletApplicationService : IManageWalletUseCase, IPlaySlotUseCase
{
    private readonly Wallet _wallet = new();
    private readonly IRngEngine _rngEngine;
    private readonly object _lock = new();

    public WalletApplicationService(IRngEngine rngEngine)
    {
        _rngEngine = rngEngine;
    }

    public decimal GetBalance()
    {
        lock (_lock) return _wallet.Balance;
    }

    public decimal Deposit(decimal amount)
    {
        lock (_lock)
        {
            _wallet.Deposit(amount);
            return _wallet.Balance;
        }
    }

    public decimal Withdraw(decimal amount)
    {
        lock (_lock)
        {
            _wallet.Withdraw(amount);
            return _wallet.Balance;
        }
    }

    public GameResult Play(decimal betAmount)
    {
        if (betAmount < 1 || betAmount > 10)
            throw new ArgumentOutOfRangeException(nameof(betAmount), "La apuesta debe estar entre $1 y $10.");

        lock (_lock)
        {
            if (_wallet.Balance < betAmount)
                throw new InvalidOperationException("Saldo insuficiente para realizar esta apuesta.");

            // Consumimos el puerto de salida para la aleatoriedad
            var result = _rngEngine.GenerateSpinResult(betAmount);

            _wallet.ProcessBetAndWin(betAmount, result.WinAmount);
            return result;
        }
    }
}
