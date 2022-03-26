using System.Reflection;
using ControlService.ExternalModules;
using ControlService.Models;
using ControlService.Core.Models;
using System.Diagnostics;

namespace ControlService.Core
{
    public class Core
    {
        private readonly ILogger<Core> _logger;


        Dictionary<string, IExternalModule> Modules;
        Api _api;



        SettingsModel _settings;

        EventHandler<EventMessageArgs> _messager;

        public Core(ILogger<Core> logger)
        {
            _logger = logger;
            _messager += SendMessage;
            
        }

        public int Initialization()
        {
            try
            {
                FileManager<SettingsModel> manager = new FileManager<SettingsModel>("settings");
                _settings = manager.ReadFromFile("settings.json");
                _api = new Api(_settings.Guid);
                _settings.Guid = _api.Guid;
                if (_settings.ExternalModules != null)
                {
                    foreach (string module in _settings.ExternalModules)
                    {
                        IncludeModule(module);
                    }
                }
                else
                {
                    _settings.ExternalModules = new List<string>();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}", DateTimeOffset.Now);
                Trace.WriteLine($"{ex.Message} init");
                return 1;
            }

            return 0;
        }


        public async Task Work()
        {
            try
            {
                CommandsProcessing();
                await Task.Delay(_settings.Delay);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", DateTimeOffset.Now);
            }

        }

        void SaveSettings()
        {
            FileManager<SettingsModel> manager = new FileManager<SettingsModel>("settings");
            manager.CreateFile("settings.json", _settings);
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
                case "delay":
                    _settings.Delay = Convert.ToInt32(commandlets[1]);
                    SaveSettings();
                    break;
                case "includemodule":
                    if (IncludeModule(commandlets[1]) == 0)
                        _settings.ExternalModules.Add(commandlets[1]);
                    break;
                case "install":
                    InstallModule(commandlets[1]);
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
            try
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
            catch(Exception ex)
            {
                _logger.LogError($"cant start {moduleName}. {ex.Message}", DateTimeOffset.Now);
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
        int IncludeModule(string moduleName)
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
                    IExternalModule module = (IExternalModule)obj;
                    module.MessageSend = _messager;
                    Modules.Add(moduleName, module);
                    return 0;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"cant load {moduleName}", DateTimeOffset.Now);
                    _logger.LogError(ex.Message, DateTimeOffset.Now);
                }
            }

            return 1;

        }
        void InstallModule(string moduleName)
        {
            _api.DownloadFile(moduleName);
        }

        
        

        void SendMessage(object sender, EventMessageArgs args)
        {
            _logger.LogInformation($"Module {args.ModuleName} sending: {args.Text}", DateTimeOffset.Now);
            _logger.LogInformation($"Sending: {_api.SendMessage(args.Text)}", DateTimeOffset.Now);
        }
    }
}
