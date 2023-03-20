using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BasketService.Config.Connections;
public class RedisConnectionFactory : IRedisConnectionFactory
{
    private Lazy<ConnectionMultiplexer> _connection;

    public RedisConnectionFactory(IOptions<RedisSettings> options) 
        => _connection = new Lazy<ConnectionMultiplexer>(()
            => ConnectionMultiplexer.Connect($"{options.Value.Host}:{options.Value.Port}"));

    public ConnectionMultiplexer Connection() 
        => _connection.Value;

    public IDatabase GetDatabase(int db = 1) 
        => _connection.Value.GetDatabase(db);
}