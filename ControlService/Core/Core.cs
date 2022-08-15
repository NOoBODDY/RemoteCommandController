using ControlService.Models;
using ExternalModule;

namespace ControlService.Core
{
    public class Core
    {
        


        private readonly ILogger<Core> _logger;
        private readonly IConfiguration _configuration;
        private readonly SettingsService _settingsService;
        //_______________________________________________________________

        private readonly Api _api;
        private readonly CommandService _commandService;

        public Core(SettingsService settingsService, Api api, CommandService commandService, ILogger<Core> logger)
        {
            _settingsService = settingsService;
            //_messager += SendMessage;
            _api = api;
            _commandService = commandService;
            _commandService.MessageHandler = SendMessage;
            _logger = logger;
        }

        internal async Task MainItteration()
        {
            List<Command> commands = await _api.GetCommands();
            _commandService.AddCommand(commands.Select(x => x.CommandText).ToArray());
            await Task.Delay(_settingsService.Delay);
        }









        //_______________________________________________________________

        void SendMessage(object sender, EventMessageArgs args)
        {
            _logger.LogInformation($"Module {args.ModuleName} sending: {args.Text}", DateTimeOffset.Now);
            _logger.LogInformation($"Sending: {_api.SendMessage(args.Text)}", DateTimeOffset.Now);
        }
    }
}
