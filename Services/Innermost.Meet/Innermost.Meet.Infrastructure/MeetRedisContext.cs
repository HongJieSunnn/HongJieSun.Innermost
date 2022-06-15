using StackExchange.Redis;

namespace Innermost.Meet.Infrastructure
{
    public class MeetRedisContext
    {
        private readonly string _redisConnectionString;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        public MeetRedisContext(string redisConnectionString)
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
