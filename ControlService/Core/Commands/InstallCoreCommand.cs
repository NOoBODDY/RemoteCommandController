using ControlService.Core.Models;
namespace ControlService.Core.Commands
{
    [Command("install")]
    internal class InstallCoreCommand : AbstractCommand
    {
        internal override void Execute(string[] args)
        {
            ((CoreModule)_module).InstallModule(args[0]);
        }
    }
}
