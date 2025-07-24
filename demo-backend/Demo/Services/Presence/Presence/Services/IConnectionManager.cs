namespace Presence.Services
{
    public interface IConnectionManager
    {
        Task SetUserOnlineAsync(string userId, string connectionId);
        Task SetUserOfflineAsync(string userId);
        Task<string?> GetUserStatusAsync(string userId);
        Task<Dictionary<string, bool>> GetBatchStatus(List<string> userIds);
    }

}
