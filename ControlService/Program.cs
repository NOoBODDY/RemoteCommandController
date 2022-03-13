using ControlService;
using ControlService.Core;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Core>();
    })
    .Build();

await host.RunAsync();
