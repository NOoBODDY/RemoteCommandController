using ExternalModule;
namespace ControlService.Core.Models
{
    public class CoreModule : IExternalModule
    {
        private readonly SettingsService _settingsService;
        private readonly Api _api;

        public EventHandler<EventMessageArgs> MessageSend { get; set; }
        

        public CoreModule(SettingsService settingsService, Api api)
        {
            _settingsService = settingsService;
            _api = api;
        }

        public void SetDelay(int milliseconds)
        {
            _settingsService.Delay = milliseconds;
        }

        public void InstallModule(string moduleName)
        {
            _api.DownloadFile(moduleName);
        }


        public void AddCommand(string command)
        {
            throw new NotImplementedException();
        }

        public void StartModule(string[] args)
        {
            throw new NotImplementedException();
        }

        public void StopModule(string[] args)
        {
            throw new NotImplementedException();
        }



    }

}
