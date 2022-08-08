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

#if DEBUG
            Trace.WriteLine("Core was inited");
#endif
            while (!stoppingToken.IsCancellationRequested)
            {
                await _core.MainItteration();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
        }
    }

}