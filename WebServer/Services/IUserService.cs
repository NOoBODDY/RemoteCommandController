using WebServer.Models;

namespace WebServer.Services;

public interface IUserService
{
    Task<User[]> GetAdminsWithUserParams();
    Task<User> GetUserByName(string userName);
    Task<User> GetUserByNameWithRole(string userName, string password);

    Task AddUser(string userName, string password);
    Task UpdatePassword(string userName, string newPassword);
    Task UpdateUser(User user);
    Task UpdateUser(User[] users);

}