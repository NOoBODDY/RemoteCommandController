using ControlService;
using ControlService.Core;
using ControlService.Core.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET UsefulService";
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<SettingsService>();
        services.AddSingleton<Api>();
        services.AddSingleton<Core>();
        services.AddSingleton<ModuleFabric>();
        services.AddSingleton<CommandFabric>();
        services.AddSingleton<CommandService>();
        services.AddSingleton<CoreModule>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
