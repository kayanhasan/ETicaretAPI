using ETicaretAPI.Application.Abstractions.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace ETicaretAPI.Infrastructure.Services.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _redis;
        // IConnectionMultiplexer, Redis sunucusu ile olan ana bağlantıyı yönetir
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            _redis = redis;
        }

        // Önbellekten veri çekme metodu
        // Önbellekten veri çekme metodu
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            // Hatayı çözen kısım: value değerini açıkça .ToString() ile string'e çeviriyoruz
            string jsonString = value.ToString();

            return JsonSerializer.Deserialize<T>(jsonString);
        }

        // Önbelleğe veri yazma metodu
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            // Eğer özel bir süre verilmediyse varsayılan olarak 1 saat sakla
            var options = expiration ?? TimeSpan.FromHours(1);

            var serializedValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, serializedValue, options);
        }

        // Belirli bir key'i silme metodu
        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        // Key var mı kontrolü
        public async Task<bool> AnyAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            // Bağlı olan Redis sunucularının endpoint adreslerini alıyoruz (Genelde tek bir localhost olur)
            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());

            // Belirttiğimiz şablona uyan tüm anahtarları (Keys) Redis üzerinden tarıyoruz
            // Örn: pattern = "products-*" ise içinde products- geçen her şeyi bulur
            var keys = server.Keys(database: _database.Database, pattern: pattern).ToArray();

            if (keys.Length > 0)
            {
                // Bulunan tüm anahtarları tek bir hamlede Redis'ten siliyoruz
                await _database.KeyDeleteAsync(keys);
            }
        }
    }
}
