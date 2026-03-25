namespace Guild.Application.Features.Guild.Queries.GetGuilds
{
    public class GetGuildsVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
        public Guid OwnerId { get; set; }
    }
}
