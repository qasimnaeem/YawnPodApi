using StackExchange.Redis;
using System;

namespace YawnMassage.Common.Services
{
    /// <summary>
    /// This class wraps Redis ConnectionMultiplexer singleton which pools connections to Redis server.
    /// Without connection pooling, Redis reads/writes will have poor performance.
    /// Therefore this class is intended to be a singleton.
    /// </summary>
    public class RedisConnectionPoolService
    {
        private Lazy<ConnectionMultiplexer> lazyConnection;
        private Lazy<IDatabase> lazyDatabase;

        public RedisConnectionPoolService(string redisConnectionString)
        {
            SetupConnectionMultiplexer(redisConnectionString);
        }

        private void SetupConnectionMultiplexer(string connectionString)
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var options = ConfigurationOptions.Parse(connectionString);
                return ConnectionMultiplexer.Connect(options);
            });

            lazyDatabase = new Lazy<IDatabase>(() =>
                lazyConnection.Value.GetDatabase()
            );
        }

        public IDatabase GetDatabase()
        {
            return lazyDatabase.Value;
        }
    }
}
