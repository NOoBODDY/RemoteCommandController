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
            StringBuilder builder = new StringBuilder();
            builder.AppendJoin(' ', args);

            var proc = new ProcessStartInfo()
            {
                UseShellExecute = true,
                WorkingDirectory = @"C:\Windows\System32",
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = "/c " + builder.ToString(),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            System.Diagnostics.Process.Start(proc);
        }
    }
}