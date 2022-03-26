using ControlService;
using ControlService.Core;
using Microsoft.Extensions.Hosting.WindowsServices;
using ControlService.Core.Models;




IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET UsefulService";
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<Core>();
        services.AddHostedService<Worker>();
        services.Configure<SettingsModel>(context.Configuration.GetSection("SettingsModel"));
    })
    .Build();

await host.RunAsync();
