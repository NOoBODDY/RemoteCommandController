
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
            int check = _core.Initialization();
            while (stoppingToken.IsCancellationRequested || check != 0)
            {
                await Task.Delay(30000, stoppingToken);
                check = _core.Initialization();
            }

#if DEBUG
            Trace.WriteLine("Core was inited");
#endif
            while (!stoppingToken.IsCancellationRequested)
            {
                await _core.Work();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
        }
    }

}