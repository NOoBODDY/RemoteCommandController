using ExternalModule;
namespace ControlService.Core.Commands
{
    internal abstract class AbstractCommand
    {
        private protected IExternalModule _module;

        internal void AddReciever(IExternalModule module, EventHandler<EventMessageArgs> messageHandler = null)
        {
            _module = module;
            _module.MessageSend += messageHandler;
        }

        internal abstract void Execute(string[] args);
    }
}
