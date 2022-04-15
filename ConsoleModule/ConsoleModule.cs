using ControlService.ExternalModules;
using System.Diagnostics;
using System.Text;

namespace ConsoleModule
{
    public class ConsoleModule : IExternalModule
    {
        public string Name { get; set; }
        public OperationSystemEnum Target { get; set; }
        public Queue<string> CommandsPull { get; set; }
        public EventHandler<EventMessageArgs> MessageSend { get; set; }

        public StatusCodeEnum StartModule(string[] args)
        {
            this.Process(args);
            return StatusCodeEnum.Disposable;
        }

        public StatusCodeEnum StopModule(string[] args)
        {
            throw new NotImplementedException();
        }

        public ConsoleModule()
        {
            CommandsPull = new Queue<string>();
            Target = OperationSystemEnum.windows;
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