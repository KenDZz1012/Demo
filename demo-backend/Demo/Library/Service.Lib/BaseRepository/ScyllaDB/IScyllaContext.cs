using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.BaseRepository.ScyllaDB
{
    public interface IScyllaContext : IDisposable
    {
        ISession Session { get; }
        IMapper Mapper { get; }
        string Keyspace { get; }
    }
}
