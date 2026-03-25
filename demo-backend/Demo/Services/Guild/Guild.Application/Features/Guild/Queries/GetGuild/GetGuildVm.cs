namespace Guild.Application.Features.Guild.Queries.GetGuild
{
    public class GetGuildVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public string? BannerUrl { get; set; }
        public Guid OwnerId { get; set; }
        public string VerificationLevel { get; set; } = "None";
        public int MaxMembers { get; set; }
        public int MemberCount { get; set; }
    }
}
