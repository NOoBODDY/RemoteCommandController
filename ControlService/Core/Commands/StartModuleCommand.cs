namespace ControlService.Core.Commands
{
    [Command("start")]
    internal class StartModuleCommand : AbstractCommand
    {

        internal override void Execute(string[] args)
        {
            _module.StartModule(args);
        }
    }
}
