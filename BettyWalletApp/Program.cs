// Program.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BettyWalletApp.Adapters.In;
using BettyWalletApp.Ports.Out;
using BettyWalletApp.Adapters.Out.Domain.Services;
using BettyWallet.Domain.Services;
using BettyWalletApp.Ports.In;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<IRngEngine, CryptoRngAdapter>();

        services.AddSingleton<WalletApplicationService>();
        services.AddSingleton<IManageWalletUseCase>(x => x.GetRequiredService<WalletApplicationService>());
        services.AddSingleton<IPlaySlotUseCase>(x => x.GetRequiredService<WalletApplicationService>());

        services.AddTransient<ConsoleUiAdapter>();
    })
    .Build();

var uiAdapter = host.Services.GetRequiredService<ConsoleUiAdapter>();
uiAdapter.Run();
