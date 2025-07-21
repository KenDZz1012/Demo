using Channel.Domain.Entities;

namespace Channel.Application.Contracts.Persistence;

public interface IServerInviteLinkRepository
{
    Task<bool> AddAsync(ServerInviteLink server);
    Task<ServerInviteLink> GetByIdAsync(Guid id);
    Task<ServerInviteLink> CheckExistCode(string code);
}