using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Services;

public class DataBaseUserParamsForRemoteService: IUserParamsForRemoteService
{
    private readonly DataBaseContext _context;
    public DataBaseUserParamsForRemoteService(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<UserParamsForRemote[]> GetParamsByUserId(int id)
    {
        return await _context.UsersParamsForRemote.Where(u => u.UserId == id).ToArrayAsync();
    }

    public async Task<UserParamsForRemote?> GetParamsByUserIdAndComputerId(int userId, int computerId)
    {
        return  await _context.UsersParamsForRemote.FirstOrDefaultAsync(u => u.UserId == userId && u.RemoteComputerId == computerId);
    }

    public async Task SaveParams(UserParamsForRemote userParamsForRemote)
    {
        _context.UsersParamsForRemote.Update(userParamsForRemote);
        await _context.SaveChangesAsync();
    }
}