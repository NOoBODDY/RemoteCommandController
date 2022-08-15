using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Services;

public class DataBaseCommandService: ICommandService
{
    private readonly DataBaseContext _context;
    public DataBaseCommandService(DataBaseContext context) => _context = context;

    public async Task AddCommand(Command command)
    {
        _context.Commands.Add(command);
        _context.SaveChanges();
    }

    public async Task<Command[]> GetLastCommandForComputer(int id, DateTime lastConnection)
    {
        return await _context.Commands.Where(u => u.RemoteComputerId == id && u.TimeCreation > lastConnection).ToArrayAsync();
    }
}