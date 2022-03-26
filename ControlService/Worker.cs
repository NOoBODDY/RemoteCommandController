using ControlService.Core;
using System.Diagnostics;

namespace ControlService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Core.Core _core;

        public Worker(ILogger<Worker> logger, Core.Core core) => (_logger, _core) = (logger, core);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Trace.WriteLine("before Init");
            _logger.LogInformation("before Init: {time}", DateTimeOffset.Now);
            int check = _core.Initialization();
            Trace.WriteLine("after first Init");
            _logger.LogInformation("after first Init: {time}", DateTimeOffset.Now);
            while (stoppingToken.IsCancellationRequested || check != 0)
            {
                await Task.Delay(30000, stoppingToken);
                check = _core.Initialization();
            }
            _logger.LogInformation("after next Init: {time}", DateTimeOffset.Now);
            Trace.WriteLine("after next Init");
            while (!stoppingToken.IsCancellationRequested)
            {
                await _core.Work();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                Trace.WriteLine($"Worker running at: {DateTimeOffset.Now}");
                Trace.WriteLine($"Path {PupaZalupa.path}");
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
    public static class PupaZalupa
    {
        public static string path;
    }

}