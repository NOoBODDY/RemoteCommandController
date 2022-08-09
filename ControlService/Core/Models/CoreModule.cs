using ExternalModule;
namespace ControlService.Core.Models
{
    public class CoreModule : IExternalModule
    {
        public EventHandler<EventMessageArgs> MessageSend { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
