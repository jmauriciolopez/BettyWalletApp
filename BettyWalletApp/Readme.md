# Betty Wallet Core — Technical Assessment Task

Summary
- .NET 9 console project simulating a player wallet for a casino experience.

- Objective: to evaluate design, code quality, and architectural decisions (hexagonal / Ports & Adapters).

Problem Context
- The player starts with a balance of $0.

- Supported operations: deposit, withdrawal, betting, and receiving payments.

- Simple slot game with these rules:

- Bets between $1 and $10.

- 50% lose.

- 40% win up to x2.

- 10% win between x2 and x10.

- Updated balance: new = old - bet + winnings.

Requirements
- .NET SDK 9 installed.

- Solution: `BettyWalletApp.slnx`
- Main project under `BettyWalletApp` (hexagonal structure: `Adapters`, `Ports`, etc.).

Relevant structure:
- `Adapters/In/ConsoleUiAdapter.cs` — console UI adapter (user interaction).

- `Ports/In` — use case interfaces, e.g., `IManageWalletUseCase`, `IPlaySlotUseCase`.

- The adapter shows that the exposed APIs are:

- `_walletUseCase.GetBalance()`

- `_walletUseCase.Deposit(decimal amount)`

- `_walletUseCase.Withdraw(decimal amount)`

- `_slotUseCase.Play(decimal bet)` → result with `IsWin`, `Multiplier`, `WinAmount`.

How to run (command line)
1. Open a terminal in the repository root:

- `cd C:\Users\mauricio\source\repos\BettyWalletApp`
2. Compile:

- `dotnet build`
3. Run the console app (from the project containing `Program.cs` / executable):

- `dotnet run --project .\BettyWalletApp\BettyWalletApp.csproj`

How to run (Visual Studio)
- Open the `BettyWalletApp.slnx` solution in Visual Studio.

- Select the startup project in the Solution Explorer and use Run / Start Debugging.

Example of use
- Upon startup, you will see the balance (`Current Balance: $0.00`) and a menu with: `Deposit | Withdraw | Bet | Exit`.

- Depositing increases the balance; withdrawing decreases it.

- The bet prompts for an amount between 1 and 10, runs the simulation, and displays whether there was a win and the new balance.

Design Decisions (Summary)
- Hexagonal architecture (Ports & Adapters) to facilitate testing and UI/infrastructure interchangeability.

- Use cases encapsulate wallet and game logic (single-responsibility).

- Input validations in the console adapter and domain layers (do not rely solely on the UI).

- Centralized RNG and probabilities in the 'Play' use case to facilitate testing (injectable/mockable).

Testing and Extensibility
- Designed to allow unit testing of use cases (port/adapter mocks).

- To add persistence, implement an 'Adapter/Out' that meets the port interfaces.

- For metrics or concurrency limits, introduce infrastructure layers and circuit breakers as needed.

Final Notes
- Keep responsibilities simple; avoid overengineering.
- Be prepared to justify design decisions and trade-offs during the technical interview.

License / Contact
- Test repository for technical evaluation. For technical questions, review the use cases under `Ports` and the adapters under `Adapters`.