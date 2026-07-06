using BettyWalletApp.Domain.Models;
using BettyWalletApp.Ports.In;
using BettyWalletApp.Ports.Out;

namespace BettyWalletApp.Domain.Services;

public class WalletApplicationService : IManageWalletUseCase, IPlaySlotUseCase
{
    private readonly IRngEngine _rngEngine;
    private readonly IWalletRepository _walletRepository;
    private readonly object _lock = new();
    private readonly decimal _minBet;
    private readonly decimal _maxBet;

    public WalletApplicationService(IRngEngine rngEngine, IWalletRepository walletRepository, decimal minBet = 1m, decimal maxBet = 10m)
    {
        _rngEngine = rngEngine;
        _walletRepository = walletRepository;
        _minBet = minBet;
        _maxBet = maxBet;
    }

    public decimal GetBalance()
    {
        lock (_lock) return _walletRepository.Get().Balance;
    }

    public decimal Deposit(decimal amount)
    {
        lock (_lock)
        {
            var wallet = _walletRepository.Get();
            wallet.Deposit(amount);
            _walletRepository.Save(wallet);
            return wallet.Balance;
        }
    }

    public decimal Withdraw(decimal amount)
    {
        lock (_lock)
        {
            var wallet = _walletRepository.Get();
            wallet.Withdraw(amount);
            _walletRepository.Save(wallet);
            return wallet.Balance;
        }
    }

    public GameResult Play(decimal betAmount)
    {
        if (betAmount < _minBet || betAmount > _maxBet)
            throw new ArgumentOutOfRangeException(nameof(betAmount), $"The bet must be between ${_minBet} y ${_maxBet}");

        lock (_lock)
        {
            var wallet = _walletRepository.Get();
            if (wallet.Balance < betAmount)
                throw new InvalidOperationException("Insufficient funds to place this bet.");

            var result = _rngEngine.GenerateSpinResult(betAmount);

            wallet.ProcessBetAndWin(betAmount, result.WinAmount);
            _walletRepository.Save(wallet);
            return result;
        }
    }
}
