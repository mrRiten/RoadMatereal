using StackExchange.Redis;

namespace RoadMatereal.Models
{
    public class RedisStore
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisStore(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public void Set(string key, string value)
        {
            _db.StringSet(key, value);
        }

        public string Get(string key)
        {
            return _db.StringGet(key);
        }

        public bool Delete(string key)
        {
            return _db.KeyDelete(key);
        }

        public bool Exists(string key)
        {
            return _db.KeyExists(key);
        }

        public void ListRightPush(string key, string value)
        {
            _db.ListRightPush(key, value);
        }

        public string ListLeftPop(string key)
        {
            return _db.ListLeftPop(key);
        }

        public long ListLength(string key)
        {
            return _db.ListLength(key);
        }

        public RedisValue[] ListRange(string key, long start = 0, long stop = -1)
        {
            return _db.ListRange(key, start, stop);
        }
    }
}
