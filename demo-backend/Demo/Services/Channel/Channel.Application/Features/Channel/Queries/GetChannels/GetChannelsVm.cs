namespace Channel.Application.Features.Channel.Queries.GetChannels
{
    public class GetChannelsVm
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Position { get; set; }
        public string? Topic { get; set; }
        public bool Nsfw { get; set; }
    }
}
