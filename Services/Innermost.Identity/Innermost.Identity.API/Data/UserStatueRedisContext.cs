using StackExchange.Redis;

namespace Innermost.Identity.API.Data
{
    public class UserStatueRedisContext
    {
        private readonly string _redisConnectionString;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public UserStatueRedisContext(string redisConnectionString)
        {
            _redisConnectionString = redisConnectionString;

            _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConnectionString);
        }

        public IDatabase Context(int databaseNumber = 0, object? asyncState = null)
        {
            return _connectionMultiplexer.GetDatabase(databaseNumber, asyncState);
        }
    }
}
