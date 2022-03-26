using ControlService;
using ControlService.Core;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET UsefulService";
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<Core>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
