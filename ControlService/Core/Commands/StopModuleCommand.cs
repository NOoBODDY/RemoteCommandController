
namespace ControlService.Core.Commands
{
    [Command("stop")]
    internal class StopModuleCommand : AbstractCommand
    {

        internal override void Execute(string[] args)
        {
            _module.StopModule(args);
        }
    }
}
