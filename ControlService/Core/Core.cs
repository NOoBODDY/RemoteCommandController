using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ControlService.ExternalModules;
using ControlService.Models;
using ControlService.Core.Models;


namespace ControlService.Core
{
    public class Core : BackgroundService
    {
        private readonly ILogger<Core> _logger;
        Dictionary<string, IExternalModule> Modules;
        List<string> ModulesToInclude;
        static string settingsPath = "settings";
        Api _api;

        public Core(ILogger<Core> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            FileManager<SettingsModel> manager = new FileManager<SettingsModel>(settingsPath);
            SettingsModel settings = manager.ReadFromFile("settings.json");
            if (settings == null)
            {
                settings = new SettingsModel();
                manager.CreateFile("settings.json", settings);
            }
            ModulesToInclude = settings.ExternalModules;
            foreach (string module in ModulesToInclude)
            {
                IncludeModule(module);
            }
            _api = new Api(settings.Guid);
            SaveSettings();


            while (!stoppingToken.IsCancellationRequested)
            {

                CommandsProcessing();
                
                
                
                _logger.LogInformation("Core running at: {time}", DateTimeOffset.Now);
                await Task.Delay(30000, stoppingToken);
            }
        }



        void CommandsProcessing()
        {
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
                case "includemodule":
                    IncludeModule(commandlets[1]);
                    break;
                default:
                    if (Modules != null && Modules.Count != 0)
                    {
                        IExternalModule module = Modules.FirstOrDefault(u => u.Key == commandlets[0]).Value;
                        if (module != null)
                        {
                            lock (module.CommandsPull)
                            {
                                module.CommandsPull.Enqueue(command.CommandText.Skip(commandlets[0].Length + 1).ToString());
                            }
                            _logger.LogInformation($"Command added to module {module.Name}.", DateTimeOffset.Now);
                        }
                        else
                        {
                            _logger.LogInformation($"module {module.Name} not found. Command doesnt invoke", DateTimeOffset.Now);
                        }
                    }
                    _logger.LogInformation($"module or command not found", DateTimeOffset.Now);

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
        void IncludeModule(string moduleName)
        {
            if (Modules == null)
            {
                Modules = new Dictionary<string, IExternalModule>();
            }
            if (Modules.ContainsKey(moduleName))
            {
                _logger.LogInformation($"cant load {moduleName}. It is already loaded", DateTimeOffset.Now);
            }
            else
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom("External/" + moduleName + ".dll");
                    Type? t = asm.GetType($"{moduleName}.{moduleName}");
                    object? obj = Activator.CreateInstance(t);
                    Modules[moduleName] = (IExternalModule)obj;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"cant load {moduleName}", DateTimeOffset.Now);
                    _logger.LogError(ex.Message, DateTimeOffset.Now);
                }
            }
            


        }

        void SaveSettings()
        {
            FileManager<SettingsModel> manager = new FileManager<SettingsModel>(settingsPath);
            SettingsModel settings = new SettingsModel();
            settings.Guid = _api.Guid;
            settings.ExternalModules = Modules.Keys.ToList();
            manager.CreateFile("settings.json", settings);
        }

    }
}
