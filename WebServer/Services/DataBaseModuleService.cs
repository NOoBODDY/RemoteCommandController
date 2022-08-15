using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Services;

public class DataBaseModuleService:IModuleService
{
    private readonly DataBaseContext _context;
    public DataBaseModuleService(DataBaseContext context) => _context = context;

    public async Task<Module[]> GetAllModules()
    {
        return await _context.Moduls.ToArrayAsync();
    }

    public async Task<Module[]> GetAllModulesWithAuthor()
    {
        return await _context.Moduls.Include(m=> m.Author).ToArrayAsync();
    }

    public async Task<Module> GetModuleById(int id)
    {
        return await _context.Moduls.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Module> GetModuleByName(string name)
    {
        return await _context.Moduls.FirstOrDefaultAsync(m => m.Name == name);
    }

    public async Task AddNewModule(Module module)
    {
        await _context.Moduls.AddAsync(module);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateModule(Module module)
    {
        _context.Moduls.Update(module);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteModule(Module module)
    {
        _context.Moduls.Remove(module);
        await _context.SaveChangesAsync();
    }
}