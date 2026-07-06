using BettyWalletApp.Adapters.In;
using BettyWalletApp.Adapters.Out;
using BettyWalletApp.Domain.Services;
using BettyWalletApp.Ports.In;
using BettyWalletApp.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration((_, config) =>
     {
         config.SetBasePath(AppContext.BaseDirectory);
         config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
     })
    .ConfigureServices((context, services) =>
    {
        var gameSettings = context.Configuration.GetSection("GameSettings").Get<GameSettings>() ?? new GameSettings();

        services.AddSingleton(gameSettings);

        if (string.Equals(gameSettings.EngineType, "Bucket", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IRngEngine>(sp =>
                new BucketRngAdapter(gameSettings.LoseProbability, gameSettings.WinX2Probability));  
        }
        else
        {
            services.AddSingleton<IRngEngine>(sp =>
                new CryptoRngAdapter(gameSettings.LoseProbability, gameSettings.WinX2Probability));
        }
        services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();
        IServiceCollection serviceCollection = services.AddSingleton<WalletApplicationService>(sp =>
            new WalletApplicationService(sp.GetRequiredService<IRngEngine>(),
             sp.GetRequiredService<IWalletRepository>(),
                                         gameSettings.MinBet,
                                         gameSettings.MaxBet));
        services.AddSingleton<IManageWalletUseCase>(x => x.GetRequiredService<WalletApplicationService>());
        services.AddSingleton<IPlaySlotUseCase>(x => x.GetRequiredService<WalletApplicationService>());

        services.AddTransient<ConsoleUiAdapter>();
    })
    .Build();

var uiAdapter = host.Services.GetRequiredService<ConsoleUiAdapter>();
uiAdapter.Run();
