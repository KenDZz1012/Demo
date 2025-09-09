using Cassandra;
using Cassandra.Mapping;
using Service.Lib.BaseRepository.ScyllaDB;

namespace DirectMessage.Infrastructure.Data;

public class DirectMessageContext : IScyllaContext
{
    private readonly ICluster _cluster;
    private readonly ISession _session;
    private readonly IMapper _mapper;

    public ISession Session => _session;
    public IMapper Mapper => _mapper;
    public string Keyspace { get; }

    public DirectMessageContext(string[] contactPoints, string keyspace, int port = 9042)
    {
        Keyspace = keyspace;
        _cluster = Cluster.Builder()
            .AddContactPoints(contactPoints)
            .WithPort(port)
            .Build();

        _session = _cluster.Connect(keyspace);
        var mappingConfig = new MappingConfiguration();
        _mapper = new Mapper(_session, mappingConfig);
    }

    public void Dispose()
    {
        _session?.Dispose();
        _cluster?.Dispose();
    }
}