namespace Guild.Application.Features.GuildMember.Queries.GetGuildMembers
{
    public class GetGuildMembersVm
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GuildId { get; set; }
        public string? Nickname { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? JoinedAt { get; set; }
    }
}
