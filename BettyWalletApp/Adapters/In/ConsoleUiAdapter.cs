using BettyWalletApp.Ports.In;

namespace BettyWalletApp.Adapters.In
{
    public class ConsoleUiAdapter
    {
        private readonly IManageWalletUseCase _walletUseCase;
        private readonly IPlaySlotUseCase _slotUseCase;

        public ConsoleUiAdapter(IManageWalletUseCase walletUseCase, IPlaySlotUseCase slotUseCase)
        {
            _walletUseCase = walletUseCase;
            _slotUseCase = slotUseCase;
        }

        public void Run()
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("   Betty Wallet Core -  Hexagonal Arquitecture   ");
            Console.WriteLine("=================================================");

            bool running = true;
            while (running)
            {
                Console.WriteLine($"Saldo Actual: ${_walletUseCase.GetBalance():F2}");
                Console.WriteLine("1. Deposit | 2. Withdrawal | 3. Bet | 4. Exit");
                Console.Write("Submit Action: ");
                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Monto a depositar: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal dep))
                                Console.WriteLine($"✔️ Nuevo Saldo: ${_walletUseCase.Deposit(dep):F2}\n");
                            break;
                        case "2":
                            Console.Write("Monto a retirar: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal with))
                                Console.WriteLine($"✔️ Nuevo Saldo: ${_walletUseCase.Withdraw(with):F2}\n");
                            break;
                        case "3":
                            Console.Write("Apuesta ($1-$10): ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal bet))
                            {
                                var res = _slotUseCase.Play(bet);
                                if (res.IsWin) Console.WriteLine($"🎉 ¡Ganaste x{res.Multiplier:F2}! Premio: ${res.WinAmount:F2}");
                                else Console.WriteLine("😢 Perdiste la apuesta.");
                                Console.WriteLine();
                            }
                            break;
                        case "4":
                            running = false;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error: {ex.Message}\n");
                }
            }
        }
    }
}
