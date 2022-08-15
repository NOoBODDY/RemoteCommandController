using WebServer.Models;
namespace WebServer.Services;

public interface IRemoteComputerService
{
    Task<RemoteComputer?> GetComputerByIdWithModulsWithAuthorWithMessages(int id);
    Task<RemoteComputer?> GetComputerByIdWithParams(int id);
    Task<RemoteComputer?> GetComputerByIdWithModules(int id);
    Task<RemoteComputer?> GetComputerById(int id);
    Task<RemoteComputer> GetComputerByGUID(string guid);
    Task<string> GetComputerNameForUserById(int computerId, int userId);
    Task<RemoteComputer> AddNewComputer();
    Task DeleteComputerById(int id);
    Task UpdateComputer(RemoteComputer computer);
    Task UpdateConnectionTimeById(int id);
    Task AddModule(Module module, int computerId);
}