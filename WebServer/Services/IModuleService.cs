using WebServer.Models;

namespace WebServer.Services;

public interface IModuleService
{
    Task<Module[]> GetAllModules();
    Task<Module[]> GetAllModulesWithAuthor();
    Task<Module> GetModuleById(int id);
    Task<Module> GetModuleByName(string name);

    Task AddNewModule(Module module);
    Task UpdateModule(Module module);
    Task DeleteModule(Module module);
}