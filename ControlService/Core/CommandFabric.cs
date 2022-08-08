using ControlService.Core.Commands;
namespace ControlService.Core
{
    public class CommandFabric
    {
        Dictionary<string, Type> _commands;

        public CommandFabric()
        {
            SetCommandList();
        }

        private void SetCommandList()
        {
            _commands = new Dictionary<string, Type>();
            foreach (var command in GetCommands())
            {
                var attributes = command.GetCustomAttributes(false);
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is CommandAttribute commandAttribute)
                    {
                        if (commandAttribute.Commandlet != string.Empty || commandAttribute.Commandlet != null)
                            _commands.Add(commandAttribute.Commandlet, command);
                        break;
                    }
                }

            }
        }


        private Type[] GetCommands()
        {
            Type type = typeof(AbstractCommand);
            return type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type)).ToArray();
        }

        internal AbstractCommand CreateInstance(string commandlet)
        {
            if (_commands.ContainsKey(commandlet))
            {
                return (AbstractCommand)Activator.CreateInstance(_commands[commandlet]);
            }
            throw new ArgumentException($"{commandlet}: command not found");
        }
    }
}
