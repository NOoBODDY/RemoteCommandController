using ControlService.Core.Commands;


namespace ControlService.Core
{
    public class CommandService
    {
        private readonly ModuleFabric _moduleFabric;

        private readonly CommandFabric _commandFabric;


        private Queue<string> _commands;
        private Task _task;

        public CommandService(ModuleFabric moduleFabric, CommandFabric commandFabric)
        {
            _moduleFabric = moduleFabric;
            _commandFabric = commandFabric;
            _commands = new Queue<string>();
            _task = Task.Factory.StartNew(CycleExecute);

        }

        internal void AddCommand(string fullCommand)
        {
            lock (_commands)
            {
                _commands.Enqueue(fullCommand);
            }
            Execute();
        }
        internal void AddCommand(string[] fullCommands)
        {
            lock (_commands)
            {
                foreach (string fullCommand in fullCommands)
                    _commands.Enqueue(fullCommand);
            }
            Execute();
        }

        private void Execute()
        {
            if (_task.Status != TaskStatus.Running)
            {
                _task = Task.Factory.StartNew(CycleExecute);
            }
        }


        private void CycleExecute()
        {
            while (_commands.Any())
            {
                string command;
                lock (_commands)
                {
                    command = _commands.Dequeue();
                }
                DoCommand(command);
            }
        }



        private void DoCommand(string fullCommand)
        {
            string[] commandlets = fullCommand.Split(' ');
            string moduleName = commandlets[0];
            string commandlet = commandlets[1];
            string[] arguments = commandlets.Skip(2).ToArray();

            AbstractCommand command = _commandFabric.CreateInstance(commandlet);
            command.AddReciever(_moduleFabric.CreateInstance(moduleName));

            command.Execute(arguments);

        }


    }
}
