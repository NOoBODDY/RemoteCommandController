
namespace ExternalModule
{
    public interface IExternalModule
    {
        /// <summary>
        /// Function to start module work.
        /// Will be called after getting command "startmodule {this.Name} <args>".
        /// </summary>
        /// <param name="args">start parameters for module. Will be declared with command "startmodule"</param>
        void StartModule(string[] args);
        /// <summary>
        /// Function to stop module work and dispose all objects.
        /// Will be called after getting command "stopmodule {this.Name} <args>"
        /// </summary>
        /// <param name="args">stop parameters for module. Will be declared with command "stopmodule"</param>
        void StopModule(string[] args);

        void AddCommand(string command);


    }
}
