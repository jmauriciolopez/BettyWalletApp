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
                Console.WriteLine($"Current Balance: ${_walletUseCase.GetBalance():F2}");
                Console.WriteLine("1. Deposit | 2. Withdrawal | 3. Bet | 4. Exit");
                Console.Write("Submit Action: ");
                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Amount to deposit: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal dep))
                            {
                                _walletUseCase.Deposit(dep);
                                Console.WriteLine($"Your deposit of ${dep} was successful.\n");
                            }
                            break;
                        case "2":
                            Console.Write("Amount to withdraw: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal with))
                            {
                                _walletUseCase.Withdraw(with);
                                Console.WriteLine($"Your withdraw of ${with} was successful.\n");
                            }
                            break;
                        case "3":
                            Console.Write("Bet ($1-$10): ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal bet))
                            {
                                var res = _slotUseCase.Play(bet);
                                if (res.IsWin) Console.WriteLine($"🎉 ¡You won x{res.Multiplier:F2}! Prize: ${res.WinAmount:F2}");
                                else Console.WriteLine("You lost the bet.");
                                Console.WriteLine();
                            }
                            break;
                        case "4":
                            running = false;
                            Console.WriteLine("Thank you for playing! Hope to see you again soon.");
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
