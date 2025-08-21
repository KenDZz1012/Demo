namespace Channel.Application.Features.Server.Queries.GetServer;

public class GetServerVm
{
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string? IconUrl { get; set; }
}