using ControlService.Models;
using ExternalModule;

namespace ControlService.Core
{
    public class Core : IExternalModule
    {
        //TODO: answers from modules


        private readonly ILogger<Core> _logger;
        private readonly IConfiguration _configuration;
        private readonly SettingsService _settingsService;
        private readonly string _baseUrl;


        Dictionary<string, IExternalModule> Modules;



        SettingsService _settings;
        //_______________________________________________________________

        private readonly Api _api;
        private readonly CommandService _commandService;

        public Core(SettingsService settingsService, Api api, CommandService commandService)
        {
            _settingsService = settingsService;
            //_messager += SendMessage;
            _api = api;
            _commandService = commandService;
        }

        internal async Task MainItteration()
        {
            List<Command> commands = await _api.GetCommands();
            _commandService.AddCommand(commands.Select(x => x.CommandText).ToArray());
            await Task.Delay(_settingsService.Delay);
        }









        //_______________________________________________________________

        //void SendMessage(object sender, EventMessageArgs args)
        //{
        //    _logger.LogInformation($"Module {args.ModuleName} sending: {args.Text}", DateTimeOffset.Now);
        //    _logger.LogInformation($"Sending: {_api.SendMessage(args.Text)}", DateTimeOffset.Now);
        //}

        public void StartModule(string[] args)
        {
            throw new NotImplementedException();
        }

        public void StopModule(string[] args)
        {
            throw new NotImplementedException();
        }

        public void AddCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}
