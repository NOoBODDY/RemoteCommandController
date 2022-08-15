using WebServer.Models;
namespace WebServer.Services;

public interface IUserParamsForRemoteService
{
    Task<UserParamsForRemote[]> GetParamsByUserId(int id);
    Task<UserParamsForRemote?> GetParamsByUserIdAndComputerId(int userId, int computerId);
    Task SaveParams(UserParamsForRemote userParamsForRemote);
}