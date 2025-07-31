namespace Presence.Services
{
    public interface IConnectionManager
    {
        Task SetUserOnlineAsync(string userId, string connectionId);
        Task SetUserOfflineAsync(string userId, string connectionId);
        Task<string?> GetUserStatusAsync(string userId);
        Task<Dictionary<string, bool>> GetBatchStatus(List<string> userIds);
        Task<List<string>> GetConnectionIdsAsync(string userId);
    }

}
