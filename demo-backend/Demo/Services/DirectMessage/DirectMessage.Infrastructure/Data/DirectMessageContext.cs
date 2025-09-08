using Cassandra;

namespace DirectMessage.Infrastructure.Data;

public class DirectMessageContext : IDisposable
{
    private readonly ICluster _cluster;
    private readonly ISession _session;

    public ISession Session => _session;

    public DirectMessageContext(string[] contactPoints, string keyspace)
    {
        _cluster = Cluster.Builder()
            .AddContactPoints(contactPoints)
            .Build();

        _session = _cluster.Connect(keyspace);
    }

    public void Dispose()
    {
        _session?.Dispose();
        _cluster?.Dispose();
    }
}