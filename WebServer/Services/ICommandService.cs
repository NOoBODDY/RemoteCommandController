using WebServer.Models;

namespace WebServer.Services;

public interface ICommandService
{
    Task AddCommand(Command command);
    Task<Command[]> GetLastCommandForComputer(int id, DateTime lastConnection);
}