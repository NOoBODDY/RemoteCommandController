using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlService.ExternalModules;
using ControlService.Models;
using ControlService.Core.Models;

namespace ControlService.Core
{
    internal class Core : BackgroundService
    {
        private readonly ILogger<Core> _logger;
        Dictionary<string, IExternalModule> Modules;
        static string settingsPath = "settings";
        Api _api;

        internal Core(ILogger<Core> logger)
        {
            _logger = logger;
            FileManager<SettingsModel> manager = new FileManager<SettingsModel>(settingsPath);
            SettingsModel settings = manager.ReadFromFile("settings.json");
            if (settings == null)
            {
                settings = new SettingsModel();
                manager.CreateFile("settings.json", settings);
            }
            Modules = settings.ExternalModules;
            _api = new Api(settings.Guid);
            settings.Guid = _api.Guid;
            manager.CreateFile("settings.json", settings);

            //TimerCallback tm = new TimerCallback(CommandsProcessing);
            //Timer timer = new Timer(tm, null, 0, 30000);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                CommandsProcessing();
                
                
                
                _logger.LogInformation("Core running at: {time}", DateTimeOffset.Now);
                await Task.Delay(30000, stoppingToken);
            }
        }



        void CommandsProcessing()
        {
            //_logger.LogInformation("Timer tick", DateTimeOffset.Now);
            Task<List<Command>> task = _api.GetCommands();
            task.Wait();
            List<Command> commands = task.Result;
            foreach (var command in commands)
            {
                try
                {
                    ParseCommand(command);
                }
                catch( Exception ex)
                {
                    _logger.LogError(ex, "command skipped", DateTimeOffset.Now);
                }
                
            }
        }

        void ParseCommand(Command command)
        {
            string[] commandlets = command.CommandText.Split(' ');
            switch (commandlets[0])
            {
                case "startmodule":
                    StartModule(commandlets[1], commandlets.Skip(2).ToArray());
                    break;
                case "stopmodule":
                    StopModule(commandlets[1], commandlets.Skip(2).ToArray());
                    break;
                default:
                    IExternalModule module = Modules.FirstOrDefault(u => u.Key == commandlets[0]).Value;
                    if (module != null)
                    {
                        lock(module.CommandsPull)
                        {
                            module.CommandsPull.Enqueue(command.CommandText.Skip(commandlets[0].Length + 1).ToString());
                        }
                        _logger.LogInformation($"Command added to module {module.Name}.", DateTimeOffset.Now);
                    }
                    else
                    {
                        _logger.LogInformation($"module {module.Name} not found. Command doesnt invoke", DateTimeOffset.Now);
                    }

                    break;
            }
        }

        void StartModule(string moduleName, string[] args)
        {
            IExternalModule module = Modules.FirstOrDefault(u => u.Key == moduleName).Value;
            if (module != null)
            {
                _logger.LogInformation($"starting module {moduleName}", DateTimeOffset.Now);
                module.StartModule(args);
            }
            else
            {
                _logger.LogInformation($"module {moduleName} not found", DateTimeOffset.Now);
            }
        }
        void StopModule(string moduleName, string[] args)
        {
            IExternalModule module = Modules.FirstOrDefault(u => u.Key == moduleName).Value;
            if (module != null)
            {
                _logger.LogInformation($"stopping module {moduleName}", DateTimeOffset.Now);
                module.StopModule(args);
            }
            else
            {
                _logger.LogInformation($"module {moduleName} not found", DateTimeOffset.Now);
            }
            
        }

    }
}
