using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.Repositories;

namespace WebServer.Services;

public class DataBaseUserService: IUserService
{
    private readonly DataBaseContext _context;

    public DataBaseUserService(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<User[]> GetAdminsWithUserParams()
    {
        return await _context.Users.Include(u => u.Role).Include(t => t.UserParamsForRemotes)
            .Where(r => r.Role.Name == "admin").ToArrayAsync();
    }

    public async Task<User> GetUserByName(string userName)
    {
        return await  _context.Users.FirstOrDefaultAsync(u => u.Name == userName);
    }

    public async Task<User> GetUserByNameWithRole(string userName, string password)
    {
        return  await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Name == userName && u.Password == password);
    }

    public async Task AddUser(string userName, string password)
    {
        var user = new User { Name = userName, Password = password };
        Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
        if (userRole != null)
            user.Role = userRole;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePassword(string userName, string newPassword)
    {
        User user = await GetUserByName(userName);
        user.Password = newPassword; 
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User[] users)
    {
        _context.Users.UpdateRange(users);
        await _context.SaveChangesAsync();
    }
}