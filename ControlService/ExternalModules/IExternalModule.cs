using ControlService.Models;


namespace ControlService.ExternalModules
{
    public interface IExternalModule
    {
        /// <summary>
        /// Name of externalmodule. It will be use to parse input command.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Operation system name, where module can be used.
        /// </summary>
        public OperationSystemEnum Target { get; set; }
        /// <summary>
        /// Pull of commands to this module.
        /// </summary>
        Queue<string> CommandsPull { get; set; }
        /// <summary>
        /// Function to start module work.
        /// Will be called after getting command "startmodule {this.Name} <args>".
        /// </summary>
        /// <param name="args">start parameters for module. Will be declared with command "startmodule"</param>
        public StatusCodeEnum StartModule(string[] args);
        /// <summary>
        /// Function to stop module work and dispose all objects.
        /// Will be called after getting command "stopmodule {this.Name} <args>"
        /// </summary>
        /// <param name="args">stop parameters for module. Will be declared with command "stopmodule"</param>
        public StatusCodeEnum StopModule(string[] args);

    }
}
