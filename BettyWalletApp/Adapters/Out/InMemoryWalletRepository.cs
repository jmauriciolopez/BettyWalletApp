using BettyWalletApp.Domain.Models;
using BettyWalletApp.Ports.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettyWalletApp.Adapters.Out
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private readonly Wallet _wallet = new();
        private readonly object _lock = new();

        public Wallet Get()
        {
            lock (_lock)
            {
                return _wallet;
            }
        }

        public void Save(Wallet wallet)
        {
        }
    }
}
