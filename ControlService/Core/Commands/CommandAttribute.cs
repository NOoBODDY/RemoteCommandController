namespace ControlService.Core.Commands
{
    internal class CommandAttribute : Attribute
    {
        internal string Commandlet { get; }
        internal CommandAttribute(string commandlet)
        {
            Commandlet = commandlet;
        }
    }
}
