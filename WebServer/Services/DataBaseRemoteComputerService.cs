using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Repositories;
namespace WebServer.Services;

public class DataBaseRemoteComputerService: IRemoteComputerService
{
    private readonly DataBaseContext _context;
    public DataBaseRemoteComputerService(DataBaseContext context) => _context = context;


    public async Task<RemoteComputer?> GetComputerByIdWithModulsWithAuthorWithMessages(int id)
    {
        return await _context.RemoteComputers.Include(c=> c.Modules).
            ThenInclude(u=>u.Author).
            Include(m=> m.Messages).
            FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<RemoteComputer?> GetComputerByIdWithParams(int id)
    {
        return await _context.RemoteComputers.Include(comp => comp.UserParamsForRemotes)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<RemoteComputer?> GetComputerByIdWithModules(int id)
    {
        return await _context.RemoteComputers.Include(comp => comp.Modules)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<RemoteComputer?> GetComputerById(int id)
    {
        return await _context.RemoteComputers.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<RemoteComputer> GetComputerByGUID(string guid)
    {
        return  await _context.RemoteComputers.FirstOrDefaultAsync(u => u.GUID == guid);
    }

    public async Task<RemoteComputer> AddNewComputer()
    {
        RemoteComputer? computer = new RemoteComputer
        {
            GUID = Guid.NewGuid().ToString(),
            LastConnection = DateTime.UtcNow
        };
        _context.RemoteComputers.Add(computer);
        await _context.SaveChangesAsync();
        return computer;
    }

    public async Task DeleteComputerById(int id)
    {
        RemoteComputer computer = await GetComputerById(id);
        _context.RemoteComputers.Remove(computer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateComputer(RemoteComputer computer)
    {
        _context.RemoteComputers.Update(computer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateConnectionTimeById(int id)
    {
        var computer = await GetComputerById(id);
        computer.LastConnection = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task AddModule(Module module, int computerId)
    {
        RemoteComputer computer = await GetComputerByIdWithModules(computerId);
        computer.Modules.Add(module);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetComputerNameForUserById(int computerId, int userId)
    {
        RemoteComputer computer = await _context.RemoteComputers.Include(comp => comp.UserParamsForRemotes)
            .FirstOrDefaultAsync(u => u.Id == computerId);
        return computer.UserParamsForRemotes.FirstOrDefault(p => p.UserId == userId).ComputerName;
    }
    
    
}