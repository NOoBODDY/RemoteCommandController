using ControlService.Core.Models;

namespace ControlService.Core.Commands
{
    [Command("delay")]
    internal class DelayCoreCommand : AbstractCommand
    {

        internal override void Execute(string[] args)
        {
            ((CoreModule)_module).SetDelay(Convert.ToInt32(args[0]));
        }
    }
}
