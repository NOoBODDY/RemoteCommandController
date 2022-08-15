using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Services;

public interface IMessageService
{
    Task AddNewMessage(Message message);
}

public class DataBaseMessageService : IMessageService
{
    private readonly DataBaseContext _context;

    public DataBaseMessageService(DataBaseContext context) => _context = context;

    public async Task AddNewMessage(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }
}